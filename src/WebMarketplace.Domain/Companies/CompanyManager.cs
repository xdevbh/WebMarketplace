using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace WebMarketplace.Companies;

public class CompanyManager : DomainService
{
    private readonly IRepository<Company, Guid> _companyRepository;

    public CompanyManager(IRepository<Company, Guid> companyRepository)
    {
        _companyRepository = companyRepository;
    }

    #region Verify
    
    public async Task VerifyNameAsync(string name, Guid id)
    {
        if (await _companyRepository.AnyAsync(o => o.Name == name && o.Id != id))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyNameAlreadyExists)
                .WithData("Name", name);
        }
    }
    
    public async Task VerifyDisplayNameAsync(string displayName,Guid id)
    {
        if (await _companyRepository.AnyAsync(x => x.DisplayName == displayName && x.Id != id))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyDisplayNameAlreadyExists)
                .WithData("DisplayName", displayName);
        }
    }
    
    #endregion

    public async Task<Company> CreateAsync(
        string name, 
        string displayName,
        Guid addressId, 
        string? description, 
        string? website)
    {
        var id = GuidGenerator.Create();
        await VerifyNameAsync(name, id);
        await VerifyDisplayNameAsync(displayName, id);

        var company = new Company(
            id,
            name,
            displayName,
            addressId,
            description,
            website);
        return company;
    }
    
    public async Task EditAsync(
        Company company,
        string name, 
        string displayName,
        Guid addressId, 
        string? description, 
        string? website)
    {
        await VerifyNameAsync(name, company.Id);
        await VerifyDisplayNameAsync(displayName, company.Id);

        company.SetName(name);
        company.SetDisplayName(displayName);
        company.AddressId = addressId;
        company.Description = description;
        company.Website = website;
    }
}