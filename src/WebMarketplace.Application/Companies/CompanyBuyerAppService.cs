using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Companies;

[AllowAnonymous]
public class CompanyBuyerAppService : WebMarketplaceAppService, ICompanyBuyerAppService
{
    private readonly IRepository<Company, Guid> _companyRepository;

    public CompanyBuyerAppService(IRepository<Company, Guid> companyRepository)
    {
        _companyRepository = companyRepository;
    }

    // public async Task<ListResultDto<CompanyDto>> GetAllAsync()
    // {
    //     var vendorList = await _vendorRepository.GetListAsync();
    //     var vendorListDto = ObjectMapper.Map<List<Vendor>, List<CompanyDto>>(vendorList);
    //     return new ListResultDto<CompanyDto>(vendorListDto);
    // }
    [AllowAnonymous]
    public async Task<PagedResultDto<CompanyDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var vendorList = await _companyRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
        var totalCount = await _companyRepository.GetCountAsync();
        
        return new PagedResultDto<CompanyDto>(
            totalCount,
            ObjectMapper.Map<List<Company>, List<CompanyDto>>(vendorList)
        );
    }
    
    [AllowAnonymous]
    public async Task<ListResultDto<CompanySelectItemDto>> GetSelectItemListAsync()
    {
        var companies = await _companyRepository.GetListAsync();
        var dtos = companies.Select(c => new CompanySelectItemDto()
        {
            Id = c.Id,
            DisplayName = c.DisplayName
        }).ToList();
        
        return new ListResultDto<CompanySelectItemDto>(dtos);
    }
    
    [AllowAnonymous]
    public async Task<CompanyDto> GetAsync(Guid id)
    {
        var vendor = await _companyRepository.GetAsync(id);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }
    
    [AllowAnonymous]
    public async Task<CompanyDto> GetByNameAsync(string name)
    {
        var vendor = await _companyRepository.GetAsync(x=>x.Name == name);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;    
    }
}