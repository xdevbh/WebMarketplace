using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Companies;

public interface ICompanySellerAppService : IApplicationService
{
    // Task<ListResultDto<CompanyDto>> GetAllAsync();
    // Task<PagedResultDto<CompanyDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    // Task<CompanyDto> GetByNameAsync(string name);

    Task<CompanyDto> GetAsync();
    Task<CompanyLookupDto> GetCompanyLookupAsync();
    Task<CompanyDto> UpdateAsync(UpdateCompanySellerDto input);

    // images
    Task<CompanyImageDto> GetDefaultImageAsync();
    Task<ListResultDto<CompanyImageDto>> GetAllImagesAsync();

    Task AddImageAsync(CreateCompanyImageDto input);

    Task SetDefaultImageAsync(UpdateCompanyImageDto input);

    Task DeleteImageAsync(string blobName);
    
    // blog post 
    Task<PagedResultDto<CompanyBlogPostDto>> GetBlogPostListAsync(PagedAndSortedResultRequestDto input);
    Task<CompanyBlogPostDto> GetBlogPostAsync(Guid id);
    Task CreateBlogPostAsync(CreateUpdateCompanyBlogPostSellerDto input);
    Task UpdateBlogPostAsync(Guid id, CreateUpdateCompanyBlogPostSellerDto input);
    Task PublishBlogPostAsync(Guid blogPostId);
    Task UnpublishBlogPostAsync(Guid blogPostId);
}