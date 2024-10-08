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

    public async Task VerifyCompanyIdentificationNumberAsync(string companyIdentificationNumber, Guid id)
    {
        if (await _companyRepository.AnyAsync(o =>
                o.CompanyIdentificationNumber == companyIdentificationNumber && o.Id != id))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyIdentificationNumberAlreadyExists)
                .WithData("CompanyIdentificationNumber", companyIdentificationNumber);
        }
    }

    public async Task VerifyNameAsync(string name, Guid id)
    {
        if (await _companyRepository.AnyAsync(o => o.Name == name && o.Id != id))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyNameAlreadyExists)
                .WithData("Name", name);
        }
    }

    public async Task VerifyDisplayNameAsync(string displayName, Guid id)
    {
        if (await _companyRepository.AnyAsync(x => x.DisplayName == displayName && x.Id != id))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyDisplayNameAlreadyExists)
                .WithData("DisplayName", displayName);
        }
    }

    #endregion

    public async Task<Company> CreateAsync(
        string identificationNumber,
        string name,
        string displayName,
        Guid addressId,
        string? shortDescription,
        string? fullDescription,
        string? website)
    {
        var id = GuidGenerator.Create();

        await VerifyCompanyIdentificationNumberAsync(identificationNumber, id);
        await VerifyNameAsync(name, id);
        await VerifyDisplayNameAsync(displayName, id);

        var company = new Company(
            id,
            identificationNumber,
            name,
            displayName,
            addressId,
            shortDescription,
            fullDescription,
            website);
        return company;
    }

    public async Task EditAsync(
        Company company,
        string identificationNumber,
        string name,
        string displayName,
        Guid addressId,
        string? shortDescription,
        string? fullDescription,
        string? website)
    {
        await VerifyCompanyIdentificationNumberAsync(identificationNumber, company.Id);
        await VerifyNameAsync(name, company.Id);
        await VerifyDisplayNameAsync(displayName, company.Id);

        company.SetIdentificationNumber(identificationNumber);
        company.SetName(name);
        company.SetDisplayName(displayName);
        company.AddressId = addressId;
        company.ShortDescription = shortDescription;
        company.FullDescription = fullDescription;
        company.Website = website;
    }
}