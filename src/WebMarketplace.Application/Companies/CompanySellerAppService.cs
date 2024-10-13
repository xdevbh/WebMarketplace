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


    public async Task<CompanyDto> GetAsync()
    {
        var userId = CurrentUser.GetId();
        var membership = await _companyMembershipRepository.GetAsync(x => x.UserId == userId);
        var company = await _companyRepository.GetAsync(membership.CompanyId);

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


    public async Task<CompanyDto> UpdateAsync(UpdateCompanySellerDto input)
    {
        var userId = CurrentUser.GetId();
        var membership = await _companyMembershipRepository.GetAsync(x => x.UserId == userId);
        var company = await _companyRepository.GetAsync(membership.CompanyId);

        company.ShortDescription = input.ShortDescription;
        company.FullDescription = input.FullDescription;
        company.Website = input.Website;

        await _companyRepository.UpdateAsync(company);

        var companyDto = ObjectMapper.Map<Company, CompanyDto>(company);
        return companyDto;
    }
}