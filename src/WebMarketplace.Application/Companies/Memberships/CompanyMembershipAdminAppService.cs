using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies.Memberships;

[Authorize("AdminOnly")]
public class CompanyMembershipAdminAppService : WebMarketplaceAppService, ICompanyMembershipAdminAppService
{
    private readonly ICompanyMembershipRepository _companyMembershipRepository;
    private readonly CompanyMembershipManager _companyMembershipManager;

    public CompanyMembershipAdminAppService(ICompanyMembershipRepository companyMembershipRepository, CompanyMembershipManager companyMembershipManager)
    {
        _companyMembershipRepository = companyMembershipRepository;
        _companyMembershipManager = companyMembershipManager;
    }

    public async Task<CompanyMembershipDto> GetAsync(Guid id)
    {
        var item = await _companyMembershipRepository.GetCompanyMembershipDetailAsync(id);
        var dto = ObjectMapper.Map<CompanyMembershipDetailQueryResultItem, CompanyMembershipDto>(item);
        return dto;
    }

    public async Task<PagedResultDto<CompanyMembershipDto>> GetListAsync(CompanyMembershipFilterDto input)
    {
        var totalCount = await _companyMembershipRepository.GetCompanyMembershipDetailCountAsync(
            input.CompanyId, 
            input.UserId);
        
        var items = await _companyMembershipRepository.GetCompanyMembershipDetailListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.CompanyId,
            input.UserId
        );
        
        var dtos = ObjectMapper.Map<List<CompanyMembershipDetailQueryResultItem>, List<CompanyMembershipDto>>(items);
        return new PagedResultDto<CompanyMembershipDto>(totalCount, dtos);
    }

    public async Task<CompanyMembershipDto> CreateAsync(CreateCompanyMembershipDto input)
    {
        await _companyMembershipManager.JoinAsync(input.CompanyId, input.UserId);
        var item = await _companyMembershipRepository.GetCompanyMembershipDetailAsync(input.CompanyId, input.UserId);
        var dto = ObjectMapper.Map<CompanyMembershipDetailQueryResultItem, CompanyMembershipDto>(item);
        return dto;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _companyMembershipRepository.DeleteAsync(id);
    }
}