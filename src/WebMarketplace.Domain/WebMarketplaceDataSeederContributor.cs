using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;
using WebMarketplace.Vendors;

namespace WebMarketplace;

public class WebMarketplaceDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Vendor, Guid> _vendorRepository;

    public WebMarketplaceDataSeederContributor(
        IRepository<ProductCategory, Guid> productCategoryRepository,
        IRepository<Product, Guid> productRepository,
        IRepository<Vendor, Guid> vendorRepository)
    {
        _productRepository = productRepository;
        _vendorRepository = vendorRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _productCategoryRepository.GetCountAsync() <= 0)
        {
            await _productCategoryRepository.InsertAsync(
                new ProductCategory
                {
                    Name = "Test Category 1"
                },
                true
            );
        }

        if (await _vendorRepository.GetCountAsync() <= 0)
        {
            await _vendorRepository.InsertAsync(
                new Vendor
                {
                    Name = "Test Organization 1",
                    Address = "Test Address 1"
                },
                true
            );
        }

        if (await _productRepository.GetCountAsync() <= 0)
        {
            await _productRepository.InsertAsync(
                new Product
                {
                    Name = "Test Product 1",
                    Price = 100,
                    Currency = "USD",
                    VendorId = (await _vendorRepository.GetListAsync()).First().Id
                },
                true
            );
        }
    }
}