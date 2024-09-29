using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using WebMarketplace.Addresses;
using WebMarketplace.Vendors;

namespace WebMarketplace;

public class WebMarketplaceDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<Vendor, Guid> _vendorRepository;
    private readonly VendorManager _vendorManager;
    private readonly IRepository<Address, Guid> _addressRepository;

    public WebMarketplaceDataSeederContributor(IGuidGenerator guidGenerator, IRepository<Vendor, Guid> vendorRepository, VendorManager vendorManager, IRepository<Address, Guid> addressRepository)
    {
        _guidGenerator = guidGenerator;
        _vendorRepository = vendorRepository;
        _vendorManager = vendorManager;
        _addressRepository = addressRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedAddressesAsync();
        await SeedVendorsAsync();
    }

    private async Task SeedAddressesAsync()
    {
        if (await _addressRepository.GetCountAsync() <= 0)
        {
            var address = new Address(
                _guidGenerator.Create(),
                "Name",
                "Country",
                "State",
                "City",
                "Steet 1",
                null,
                "10000",
                null,
                "+420 000 000 000",
                "mail@mail.com");
            
            await _addressRepository.InsertAsync(address);
        }
    }

    private async Task SeedVendorsAsync()
    {
        if (await _vendorRepository.GetCountAsync() <= 0)
        {
            var address = await _addressRepository.FirstOrDefaultAsync(x => x != null);

            for (int i = 1; i <= 5; i++)
            {
                var vendor = await _vendorManager.CreateAsync(
                    "Test Vendor " + i,
                    "Test Vendor " + i,
                    address.Id,
                    "Text about Test Vendor " + i,
                    $"www.testvendor{i}.com"
                );
                await _vendorRepository.InsertAsync(vendor);
            }
        }
    }
}