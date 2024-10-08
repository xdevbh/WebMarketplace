using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Companies;

[AllowAnonymous]
public class CompanyBuyerAppService : WebMarketplaceAppService, ICompanyBuyerAppService
{
    private readonly IRepository<Company, Guid> _vendorRepository;

    public CompanyBuyerAppService(IRepository<Company, Guid> vendorRepository)
    {
        _vendorRepository = vendorRepository;
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
        var vendorList = await _vendorRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
        var totalCount = await _vendorRepository.GetCountAsync();
        
        return new PagedResultDto<CompanyDto>(
            totalCount,
            ObjectMapper.Map<List<Company>, List<CompanyDto>>(vendorList)
        );
    }
    
    [AllowAnonymous]
    public async Task<CompanyDto> GetAsync(Guid id)
    {
        var vendor = await _vendorRepository.GetAsync(id);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;
    }
    
    [AllowAnonymous]
    public async Task<CompanyDto> GetByNameAsync(string name)
    {
        var vendor = await _vendorRepository.GetAsync(x=>x.Name == name);
        var vendorDto = ObjectMapper.Map<Company, CompanyDto>(vendor);
        return vendorDto;    
    }
}