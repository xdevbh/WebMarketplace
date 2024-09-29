using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using WebMarketplace.Permissions;
using WebMarketplace.Vendors;

namespace WebMarketplace.Vendors;

// [Authorize("AdminOnly")]
public class VendorAdminAppService : WebMarketplaceAppService, IVendorAdminAppService
{
    private readonly IRepository<Vendor, Guid> _vendorRepository;
    private readonly VendorManager _vendorManager;

    public VendorAdminAppService(IRepository<Vendor, Guid> vendorRepository, VendorManager vendorManager)
    {
        _vendorRepository = vendorRepository;
        _vendorManager = vendorManager;
    }

    // public async Task<ListResultDto<VendorDto>> GetAllAsync()
    // {
    //     var vendorList = await _vendorRepository.GetListAsync();
    //     var vendorListDto = ObjectMapper.Map<List<Vendor>, List<VendorDto>>(vendorList);
    //     return new ListResultDto<VendorDto>(vendorListDto);
    // }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    [Authorize("AdminOnly")]
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
    public async Task<VendorDto> UpdateAsync(Guid id, CreateUpdateVendorAdminDto input)
    {
        var vendor = await _vendorRepository.GetAsync(id);

        await _vendorManager.EditAsync(
            vendor,
            input.Name,
            input.DisplayName,
            input.AddressId,
            input.Description,
            input.Website);

        await _vendorRepository.UpdateAsync(vendor);
        
        var vendorDto = ObjectMapper.Map<Vendor, VendorDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Create)]
    public async Task<VendorDto> CreateAsync(CreateUpdateVendorAdminDto input)
    {
        var vendor = await _vendorManager.CreateAsync(
            input.Name,
            input.DisplayName,
            input.AddressId,
            input.Description,
            input.Website
        );
        
        await _vendorRepository.InsertAsync(vendor);
        var vendorDto = ObjectMapper.Map<Vendor, VendorDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _vendorRepository.DeleteAsync(id);
    }
}