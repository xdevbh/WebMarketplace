using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using WebMarketplace.Permissions;
using WebMarketplace.Companies;

namespace WebMarketplace.Companies;

[Authorize("AdminOnly")]
public class CompanyAdminAppService : WebMarketplaceAppService, ICompanyAdminAppService
{
    private readonly IRepository<Company, Guid> _companyRepository;
    private readonly CompanyManager _companyManager;
    private readonly BlobContainer<CompanyImageContainer> _companyBlobContainer;

    public CompanyAdminAppService(IRepository<Company, Guid> companyRepository, CompanyManager companyManager, BlobContainer<CompanyImageContainer> companyBlobContainer)
    {
        _companyRepository = companyRepository;
        _companyManager = companyManager;
        _companyBlobContainer = companyBlobContainer;
    }

    // public async Task<ListResultDto<CompanyDto>> GetAllAsync()
    // {
    //     var vendorList = await _vendorRepository.GetListAsync();
    //     var vendorListDto = ObjectMapper.Map<List<Vendor>, List<CompanyDto>>(vendorList);
    //     return new ListResultDto<CompanyDto>(vendorListDto);
    // }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<PagedResultDto<CompanyDto>> GetListAsync(CompanyFilterDto input)
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
        var vendor = await _companyRepository.GetAsync(id);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Default)]
    public async Task<CompanyDto> GetByNameAsync(string name)
    {
        var vendor = await _companyRepository.GetAsync(x => x.Name == name);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Update)]
    public async Task<CompanyDto> UpdateAsync(Guid id, CreateUpdateCompanyAdminDto input)
    {
        var vendor = await _companyRepository.GetAsync(id);

        await _companyManager.EditAsync(
            vendor,
            input.CompanyIdentificationNumber,
            input.Name,
            input.DisplayName,
            input.AddressId,
            input.ShortDescription,
            input.FullDescription,
            input.Website);

        await _companyRepository.UpdateAsync(vendor);

        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Create)]
    public async Task<CompanyDto> CreateAsync(CreateUpdateCompanyAdminDto input)
    {
        var vendor = await _companyManager.CreateAsync(
            input.CompanyIdentificationNumber,
            input.Name,
            input.DisplayName,
            input.AddressId,
            input.ShortDescription,
            input.FullDescription,
            input.Website
        );

        await _companyRepository.InsertAsync(vendor);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }

    // [Authorize(WebMarketplacePermissions.Vendors.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var company = await _companyRepository.GetAsync(id);
        foreach (var image in company.Images)
        {
            await _companyBlobContainer.DeleteAsync(image.BlobName);
        }
        
        await _companyRepository.DeleteAsync(id);
    }
}