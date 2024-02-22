using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Organizations;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;

namespace WebMarketplace;

public class WebMarketplaceDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Organization, Guid> _organizationRepository;
    
    public WebMarketplaceDataSeederContributor(
        IRepository<ProductCategory, Guid> productCategoryRepository, 
        IRepository<Product, Guid> productRepository, 
        IRepository<Organization, Guid> organizationRepository)
    {
        _productRepository = productRepository;
        _organizationRepository = organizationRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _productCategoryRepository.GetCountAsync() <= 0)
        {
            await _productCategoryRepository.InsertAsync(
                new ProductCategory()
                {
                    Name = "Test Category 1"
                }, 
                autoSave: true
            );
        }
        if (await _organizationRepository.GetCountAsync() <= 0)
        {
            await _organizationRepository.InsertAsync(
                new Organization()
                {
                    Name = "Test Organization 1",
                    Address = "Test Address 1",
                }, 
                autoSave: true
            );
        }
        
        if (await _productRepository.GetCountAsync() <= 0)
        {
            await _productRepository.InsertAsync(
                new Product()
                {
                    Name = "Test Product 1",
                    Price = 100,
                    Currency = "USD",
                    OrganizationId = (await _organizationRepository.GetListAsync()).First().Id
                }, 
                autoSave: true
            );
        }
    }
}