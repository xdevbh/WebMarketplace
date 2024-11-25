using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Companies;

[AllowAnonymous]
public class CompanyBuyerAppService : WebMarketplaceAppService, ICompanyBuyerAppService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IBlobContainer<CompanyImageContainer> _companyBlobContainer;

    public CompanyBuyerAppService(ICompanyRepository companyRepository, IBlobContainer<CompanyImageContainer> companyBlobContainer)
    {
        _companyRepository = companyRepository;
        _companyBlobContainer = companyBlobContainer;
    }

    // public async Task<ListResultDto<CompanyDto>> GetAllAsync()
    // {
    //     var vendorList = await _vendorRepository.GetListAsync();
    //     var vendorListDto = ObjectMapper.Map<List<Vendor>, List<CompanyDto>>(vendorList);
    //     return new ListResultDto<CompanyDto>(vendorListDto);
    // }
    [AllowAnonymous]
    public async Task<PagedResultDto<CompanyCardDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var list = await _companyRepository.GetDetailListAsync(input.Sorting, input.MaxResultCount, input.SkipCount);
        var totalCount = await _companyRepository.GetDetailCountAsync();
        
        var dtos = new List<CompanyCardDto>();
        foreach (var item in list)
        {
            var dto = ObjectMapper.Map<CompanyDetailQueryResultItem, CompanyCardDto>(item);
            if (!item.DefaultImageBlobName.IsNullOrWhiteSpace())
            {
                dto.ImageContent = await _companyBlobContainer.GetAllBytesOrNullAsync(item.DefaultImageBlobName);
            }
            
            dtos.Add(dto);
        }
        
        return new PagedResultDto<CompanyCardDto>(
            totalCount,
            dtos
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