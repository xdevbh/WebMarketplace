using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using WebMarketplace.Addresses;
using WebMarketplace.Products;
using WebMarketplace.Companies;

namespace WebMarketplace;

public class WebMarketplaceDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<IdentityRole, Guid> _identityRoleRepository;
    private readonly IRepository<Company, Guid> _companyRepository;
    private readonly CompanyManager _companyManager;
    private readonly IRepository<Address, Guid> _addressRepository;
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly ProductManager _productManager;

    private const string DemoPrefix = "[DEMO] ";

    public WebMarketplaceDataSeederContributor(IGuidGenerator guidGenerator,
        IRepository<IdentityRole, Guid> identityRoleRepository, IRepository<Company, Guid> companyRepository,
        CompanyManager companyManager, IRepository<Address, Guid> addressRepository,
        IIdentityUserRepository identityUserRepository, IRepository<Product, Guid> productRepository,
        ProductManager productManager)
    {
        _guidGenerator = guidGenerator;
        _identityRoleRepository = identityRoleRepository;
        _companyRepository = companyRepository;
        _companyManager = companyManager;
        _addressRepository = addressRepository;
        _identityUserRepository = identityUserRepository;
        _productRepository = productRepository;
        _productManager = productManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedRolesAsync();
        await SeedAddressesAsync();
        await SeedCompaniesAsync();
        await SeedProductsAsync();
    }

    private async Task SeedRolesAsync()
    {
        if ((await _identityRoleRepository.GetCountAsync() == 1)
            && ((await _identityRoleRepository.FirstOrDefaultAsync())?.Name == "admin"))
        {
            var roleSeller = new IdentityRole(_guidGenerator.Create(), "seller");
            var roleBuyer = new IdentityRole(_guidGenerator.Create(), "buyer");

            await _identityRoleRepository.InsertAsync(roleSeller);
            await _identityRoleRepository.InsertAsync(roleBuyer);
        }
    }

    private async Task SeedAddressesAsync()
    {
        if (await _addressRepository.GetCountAsync() <= 0)
        {
            for (int i = 1; i < 10; i++)
            {
                var address = new Address(
                    _guidGenerator.Create(),
                    DemoPrefix + "Name " + i,
                    "Country " + i,
                    "State " + i,
                    "City " + i,
                    "Street " + i,
                    null,
                    $"{i}{i + 1}0000",
                    null,
                    "+420 000 000 000",
                    $"mail{i}{i}@mail.com");

                await _addressRepository.InsertAsync(address);
            }
        }
    }

    private async Task SeedCompaniesAsync()
    {
        if (await _companyRepository.GetCountAsync() <= 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                var address = await _addressRepository.FirstOrDefaultAsync(x => x.FullName == DemoPrefix + "Name " + i);

                if (address == null)
                {
                    Console.WriteLine($"Address not found for Name {i}. Skipping company creation. Run again to seed addresses first.");
                    continue; 
                }

                var company = await _companyManager.CreateAsync(
                    DemoPrefix + "Test Company " + i,
                    "Test Company " + i,
                    address.Id,
                    "Text about Test Company " + i,
                    $"www.testcompany{i}.com"
                );
                await _companyRepository.InsertAsync(company);
            }
        }
    }

    private async Task SeedProductsAsync()
    {
        if (await _productRepository.GetCountAsync() <= 0)
        {
            var user = (await _identityUserRepository.GetListAsync()).FirstOrDefault();
            var company = (await _companyRepository.GetListAsync()).FirstOrDefault();

            if (user != null && company != null)
            {
                var product = new Product(
                    user.Id,
                    company.Id,
                    DemoPrefix + "Test Product 1",
                    ProductCategory.Undefined,
                    ProductType.Goods,
                    "Test short description about Test Product 1",
                    "Test full description about Test Product 1. Test full description about Test Product 1. "
                );

                await _productRepository.InsertAsync(product);

                await _productManager.AddProductPriceAsync(product, DateTime.Now, 100, "CZK");
                await _productManager.AddProductPriceAsync(product, DateTime.Now.AddDays(-10), 200, "CZK");

                await _productManager.AddProductReviewAsync(product, user.Id, 4, "Test review about Test Product 1");
            }
        }
    }
}