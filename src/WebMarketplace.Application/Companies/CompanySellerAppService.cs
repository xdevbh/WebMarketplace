using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using WebMarketplace.Companies.Memberships;
using WebMarketplace.Permissions;

namespace WebMarketplace.Companies;

[Authorize("SellerOnly")]
public class CompanySellerAppService : WebMarketplaceAppService, ICompanySellerAppService
{
    private readonly IRepository<Company, Guid> _companyRepository;
    private readonly ICompanyMembershipRepository _companyMembershipRepository;

    public CompanySellerAppService(IRepository<Company, Guid> companyRepository,
        ICompanyMembershipRepository companyMembershipRepository)
    {
        _companyRepository = companyRepository;
        _companyMembershipRepository = companyMembershipRepository;
    }

    // public async Task<ListResultDto<CompanyDto>> GetAllAsync()
    // {
    //     var vendorList = await _vendorRepository.GetListAsync();
    //     var vendorListDto = ObjectMapper.Map<List<Vendor>, List<CompanyDto>>(vendorList);
    //     return new ListResultDto<CompanyDto>(vendorListDto);
    // }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<PagedResultDto<CompanyDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var vendorList =
            await _companyRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
        var totalCount = await _companyRepository.GetCountAsync();

        return new PagedResultDto<CompanyDto>(
            totalCount,
            ObjectMapper.Map<List<Company>, List<CompanyDto>>(vendorList)
        );
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<CompanyDto> GetAsync(Guid id)
    {
        var company = await _companyRepository.GetAsync(id);
        var companyDto = ObjectMapper.Map<Company, CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyLookupDto> GetCompanyLookupAsync()
    {
        var userId = CurrentUser.GetId();
        var membership = await _companyMembershipRepository.GetAsync(x => x.UserId == userId);
        var company = await _companyRepository.GetAsync(membership.CompanyId);
        var dto = ObjectMapper.Map<Company, CompanyLookupDto>(company);
        return dto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<CompanyDto> GetByNameAsync(string name)
    {
        var company = await _companyRepository.GetAsync(x => x.Name == name);
        var companyDto = ObjectMapper.Map<Company, CompanyDto>(company);
        return companyDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Update)]
    public async Task<CompanyDto> UpdateAsync(Guid id, UpdateCompanySellerDto input)
    {
        var company = await _companyRepository.GetAsync(id);

        company.ShortDescription = input.ShortDescription;
        company.FullDescription = input.FullDescription;
        company.Website = input.Website;

        await _companyRepository.UpdateAsync(company);

        var companyDto = ObjectMapper.Map<Company, CompanyDto>(company);
        return companyDto;
    }
}