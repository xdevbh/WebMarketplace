using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using WebMarketplace.Permissions;
using WebMarketplace.Companies;

namespace WebMarketplace.Companies;

[Authorize("AdminOnly")]
public class CompanyAdminAppService : WebMarketplaceAppService, ICompanyAdminAppService
{
    private readonly IRepository<Company, Guid> _vendorRepository;
    private readonly CompanyManager _companyManager;

    public CompanyAdminAppService(IRepository<Company, Guid> vendorRepository, CompanyManager companyManager)
    {
        _vendorRepository = vendorRepository;
        _companyManager = companyManager;
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
        var vendor = await _vendorRepository.GetAsync(x => x.Name == name);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Update)]
    public async Task<CompanyDto> UpdateAsync(Guid id, CreateUpdateCompanyAdminDto input)
    {
        var vendor = await _vendorRepository.GetAsync(id);

        await _companyManager.EditAsync(
            vendor,
            input.Name,
            input.DisplayName,
            input.AddressId,
            input.Description,
            input.Website);

        await _vendorRepository.UpdateAsync(vendor);

        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Create)]
    public async Task<CompanyDto> CreateAsync(CreateUpdateCompanyAdminDto input)
    {
        var vendor = await _companyManager.CreateAsync(
            input.Name,
            input.DisplayName,
            input.AddressId,
            input.Description,
            input.Website
        );

        await _vendorRepository.InsertAsync(vendor);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _vendorRepository.DeleteAsync(id);
    }
}