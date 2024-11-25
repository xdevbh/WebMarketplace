using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using WebMarketplace.Companies.Memberships;


namespace WebMarketplace.Companies;

[Authorize("SellerOnly")]
public class CompanySellerAppService : WebMarketplaceAppService, ICompanySellerAppService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly CompanyManager _companyManager;
    private readonly ICompanyMembershipRepository _companyMembershipRepository;
    private readonly IBlobContainer<CompanyImageContainer> _companyBlobContainer;

    public CompanySellerAppService(ICompanyRepository companyRepository, CompanyManager companyManager,
        ICompanyMembershipRepository companyMembershipRepository,
        IBlobContainer<CompanyImageContainer> companyBlobContainer)
    {
        _companyRepository = companyRepository;
        _companyManager = companyManager;
        _companyMembershipRepository = companyMembershipRepository;
        _companyBlobContainer = companyBlobContainer;
    }

    #region Company

    public async Task<CompanyDto> GetAsync()
    {
        var company = await GetMyCompany();

        var companyDto = ObjectMapper.Map<Company, CompanyDto>(company);
        return companyDto;
    }


    public async Task<CompanyLookupDto> GetCompanyLookupAsync()
    {
        var company = await GetMyCompany();

        var dto = ObjectMapper.Map<Company, CompanyLookupDto>(company);
        return dto;
    }


    public async Task<CompanyDto> UpdateAsync(UpdateCompanySellerDto input)
    {
        var company = await GetMyCompany();

        company.ShortDescription = input.ShortDescription;
        company.FullDescription = input.FullDescription;
        company.Website = input.Website;

        await _companyRepository.UpdateAsync(company);

        var companyDto = ObjectMapper.Map<Company, CompanyDto>(company);
        return companyDto;
    }

    private async Task<Company> GetMyCompany()
    {
        var userId = CurrentUser.GetId();
        var membership = await _companyMembershipRepository.GetAsync(x => x.UserId == userId);
        var company = await _companyRepository.GetAsync(membership.CompanyId);

        if (company == null)
        {
            throw new AbpAuthorizationException(L[WebMarketplaceDomainErrorCodes.CompanyNotFoundForUser]);
        }

        return company;
    }

    #endregion

    #region Images

    public async Task<CompanyImageDto> GetDefaultImageAsync()
    {
        var company = await GetMyCompany();

        if (company.Images == null || !company.Images.Any() || company.DefaultImage == null)
        {
            return new CompanyImageDto();
        }

        var dto = ObjectMapper.Map<CompanyImage, CompanyImageDto>(company.DefaultImage);
        var bytes = await _companyBlobContainer.GetAllBytesOrNullAsync(company.DefaultImage.BlobName);
        dto.Content = bytes;
        return dto;
    }

    public async Task<ListResultDto<CompanyImageDto>> GetAllImagesAsync()
    {
        var company = await GetMyCompany();

        if (company.Images == null || !company.Images.Any())
        {
            return new ListResultDto<CompanyImageDto>();
        }

        var imageDtos = new List<CompanyImageDto>();

        foreach (var image in company.Images)
        {
            var dto = ObjectMapper.Map<CompanyImage, CompanyImageDto>(image);

            var bytes = await _companyBlobContainer.GetAllBytesOrNullAsync(image.BlobName);
            dto.Content = bytes;

            imageDtos.Add(dto);
        }

        return new ListResultDto<CompanyImageDto>(imageDtos);
    }

    public async Task AddImageAsync(CreateCompanyImageDto input)
    {
        var company = await GetMyCompany();

        var blobName = GuidGenerator.Create().ToString() + "_" + input.FileName;

        if (input.Content == null || input.Content.Length <= 0)
        {
            throw new BusinessException(L[WebMarketplaceDomainErrorCodes.ImageContentIsEmpty]);
        }

        await _companyBlobContainer.SaveAsync(blobName, input.Content, overrideExisting: true);
        company.AddImage(blobName, input.IsDefault);
    }

    public async Task SetDefaultImageAsync(UpdateCompanyImageDto input)
    {
        var company = await GetMyCompany();

        company.SetDefaultImage(input.BlobName, input.IsDefault);
    }

    public async Task DeleteImageAsync(string blobName)
    {
        var company = await GetMyCompany();

        company.RemoveImage(blobName);
        await _companyBlobContainer.DeleteAsync(blobName);
    }

    #endregion

    #region BlogPosts

    public async Task<PagedResultDto<CompanyBlogPostDto>> GetBlogPostListAsync(PagedAndSortedResultRequestDto input)
    {
        var company = await GetMyCompany();
        if (company == null || company.BlogPosts == null || !company.BlogPosts.Any())
        {
            return new PagedResultDto<CompanyBlogPostDto>();
        }

        var query = company.BlogPosts.AsQueryable();
        var totalCount = company.BlogPosts.Count;
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = "CreationTime DESC";
        }

        query = query
            .OrderBy(input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
        var posts = query.ToList();
        return new PagedResultDto<CompanyBlogPostDto>(totalCount,
            ObjectMapper.Map<List<CompanyBlogPost>, List<CompanyBlogPostDto>>(posts));
    }

    public async Task<CompanyBlogPostDto> GetBlogPostAsync(Guid id)
    {
        var company = await GetMyCompany();
        if (company == null || company.BlogPosts == null || !company.BlogPosts.Any())
        {
            return new CompanyBlogPostDto();
        }

        var post = company.BlogPosts.FirstOrDefault(x => x.Id == id);
        var dto = ObjectMapper.Map<CompanyBlogPost, CompanyBlogPostDto>(post);
        return dto;
    }

    public async Task UpdateBlogPostAsync(Guid id, CreateUpdateCompanyBlogPostSellerDto input)
    {
        var company = await GetMyCompany();
        await _companyManager.EditBlogPostAsync(company, id, input.Title, input.Content);
    }

    public async Task CreateBlogPostAsync(CreateUpdateCompanyBlogPostSellerDto input)
    {
        var company = await GetMyCompany();
        await _companyManager.AddBlogPostAsync(company, input.Title, input.Content, false);
    }

    public async Task PublishBlogPostAsync(Guid blogPostId)
    {
        var company = await GetMyCompany();
        company.PublishBlogPost(blogPostId, true);
    }

    public async Task UnpublishBlogPostAsync(Guid blogPostId)
    {
        var company = await GetMyCompany();
        company.PublishBlogPost(blogPostId, false);
    }

    #endregion
}