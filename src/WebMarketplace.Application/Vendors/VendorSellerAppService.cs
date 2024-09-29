using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Permissions;
using WebMarketplace.Vendors;

namespace WebMarketplace.Vendors;

[Authorize("SellerOnly")]
public class VendorSellerAppService : WebMarketplaceAppService, IVendorSellerAppService
{
    private readonly IRepository<Vendor, Guid> _vendorRepository;

    public VendorSellerAppService(IRepository<Vendor, Guid> vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    // public async Task<ListResultDto<VendorDto>> GetAllAsync()
    // {
    //     var vendorList = await _vendorRepository.GetListAsync();
    //     var vendorListDto = ObjectMapper.Map<List<Vendor>, List<VendorDto>>(vendorList);
    //     return new ListResultDto<VendorDto>(vendorListDto);
    // }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<PagedResultDto<VendorDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var vendorList = await _vendorRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
        var totalCount = await _vendorRepository.GetCountAsync();
        
        return new PagedResultDto<VendorDto>(
            totalCount,
            ObjectMapper.Map<List<Vendor>, List<VendorDto>>(vendorList)
        );
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<VendorDto> GetAsync(Guid id)
    {
        var vendor = await _vendorRepository.GetAsync(id);
        var vendorDto = ObjectMapper.Map<Vendor, VendorDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<VendorDto> GetByNameAsync(string name)
    {
        var vendor = await _vendorRepository.GetAsync(x=>x.Name == name);
        var vendorDto = ObjectMapper.Map<Vendor, VendorDto>(vendor);
        return vendorDto;    
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Update)]
    public async Task<VendorDto> UpdateAsync(Guid id, UpdateVendorSellerDto input)
    {
        var vendor = await _vendorRepository.GetAsync(id);

        vendor.Description = input.Description;
        vendor.Website = input.Website;

        await _vendorRepository.UpdateAsync(vendor);
        
        var vendorDto = ObjectMapper.Map<Vendor, VendorDto>(vendor);
        return vendorDto;
    }
}