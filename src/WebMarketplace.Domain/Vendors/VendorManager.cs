using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace WebMarketplace.Vendors;

public class VendorManager : DomainService
{
    private readonly IRepository<Vendor, Guid> _vendorRepository;

    public VendorManager(IRepository<Vendor, Guid> vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    #region Verify
    
    public async Task VerifyNameAsync(string name)
    {
        if (await _vendorRepository.AnyAsync(o => o.Name == name))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.VendorNameAlreadyExists)
                .WithData("Name", name);
        }
    }
    
    public async Task VerifyDisplayNameAsync(string displayName)
    {
        if (await _vendorRepository.AnyAsync(x => x.DisplayName == displayName))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.VendorDisplayNameAlreadyExists)
                .WithData("DisplayName", displayName);
        }
    }
    
    #endregion

    public async Task<Vendor> CreateAsync(
        string name, 
        string displayName,
        Guid addressId, 
        string? description, 
        string? website)
    {
        await VerifyNameAsync(name);
        await VerifyDisplayNameAsync(displayName);

        var vendor = new Vendor(
            GuidGenerator.Create(), 
            name,
            displayName,
            addressId,
            description,
            website);
        return vendor;
    }
    
    public async Task EditAsync(
        Vendor vendor,
        string name, 
        string displayName,
        Guid addressId, 
        string? description, 
        string? website)
    {
        await VerifyNameAsync(name);
        await VerifyDisplayNameAsync(displayName);

        vendor.SetName(name);
        vendor.SetDisplayName(displayName);
        vendor.AddressId = addressId;
        vendor.Description = description;
        vendor.Website = website;
    }
}