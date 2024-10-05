using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Permissions;

namespace WebMarketplace.Companies;

[Authorize("SellerOnly")]
public class CompanySellerAppService : WebMarketplaceAppService, ICompanySellerAppService
{
    private readonly IRepository<Company, Guid> _vendorRepository;

    public CompanySellerAppService(IRepository<Company, Guid> vendorRepository)
    {
        _vendorRepository = vendorRepository;
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
        var vendorList = await _vendorRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
        var totalCount = await _vendorRepository.GetCountAsync();
        
        return new PagedResultDto<CompanyDto>(
            totalCount,
            ObjectMapper.Map<List<Company>, List<CompanyDto>>(vendorList)
        );
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<CompanyDto> GetAsync(Guid id)
    {
        var vendor = await _vendorRepository.GetAsync(id);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<CompanyDto> GetByNameAsync(string name)
    {
        var vendor = await _vendorRepository.GetAsync(x=>x.Name == name);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;    
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Update)]
    public async Task<CompanyDto> UpdateAsync(Guid id, UpdateCompanySellerDto input)
    {
        var vendor = await _vendorRepository.GetAsync(id);

        vendor.Description = input.Description;
        vendor.Website = input.Website;

        await _vendorRepository.UpdateAsync(vendor);
        
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }
}