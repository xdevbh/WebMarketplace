using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Currencies;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;
using WebMarketplace.Vendors;

namespace WebMarketplace;

public class WebMarketplaceDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Vendor, Guid> _vendorRepository;
    private readonly IRepository<Currency, Guid> _currencyRepository;

    public WebMarketplaceDataSeederContributor(
        IRepository<ProductCategory, Guid> productCategoryRepository,
        IRepository<Product, Guid> productRepository,
        IRepository<Vendor, Guid> vendorRepository,
        IRepository<Currency, Guid> currencyRepository)
    {
        _productRepository = productRepository;
        _vendorRepository = vendorRepository;
        _productCategoryRepository = productCategoryRepository;
        _currencyRepository = currencyRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedProductCategories();
        await SeedVendors();
        await SeedProducts();
        await SeedCurrenciesAsync();
    }

    private async Task SeedProductCategories()
    {
        if (await _productCategoryRepository.GetCountAsync() <= 0)
        {
            var baseCategories = new List<string>()
            {
                "Groceries",
                "Clothing and Footwear",
                "Appliances and Electronics",
                "Cosmetics and Fragrances",
                "Furniture and Home Decor",
                "Sporting Goods",
                "Books and Educational Materials",
                "Toys and Entertainment",
                "Household Goods",
                "Automotive Products"
            };

            
            foreach (var category in baseCategories)
            {
                await _productCategoryRepository.InsertAsync(
                    new ProductCategory
                    {
                        Name = category
                    },
                    true
                );
            }
        }
    }

    private async Task SeedVendors()
    {
        if (await _vendorRepository.GetCountAsync() <= 0)
        {
            await _vendorRepository.InsertAsync(
                new Vendor
                {
                    Name = "TechVendor sro",
                    Address = "Street 123, City, Country"
                },
                true
            );
            await _vendorRepository.InsertAsync(
                new Vendor
                {
                    Name = "FlowerVendor",
                    Address = "Street 456, City, Country"
                },
                true
            );
        }
    }
    
    private async Task SeedProducts()
    {
        if (await _productRepository.GetCountAsync() <= 0)
        {
            for (int i = 0; i < 10; i++)
            {
                await _productRepository.InsertAsync(
                    new Product
                    {
                        Name = "Some laptop " + i,
                        Price = 100,
                        Currency = "USD",
                        VendorId = (await _vendorRepository.GetListAsync()).First().Id, 
                    },
                    true
                );
            }
        }
    }
    
    private async Task SeedCurrenciesAsync()
    {
        if (await _currencyRepository.GetCountAsync() <= 0)
        {
            await _currencyRepository.InsertAsync(
                new Currency
                {
                    Name = "Euro",
                    Code = "EUR",
                    NumericCode = "978",
                    Symbol = "€"
                },
                true
            );
            await _currencyRepository.InsertAsync(
                new Currency
                {
                    Name = "US Dollar",
                    Code = "USD",
                    NumericCode = "840",
                    Symbol = "$"
                },
                true
            );
            await _currencyRepository.InsertAsync(
                new Currency
                {
                    Name = "Czech Koruna",
                    Code = "CZK",
                    NumericCode = "203",
                    Symbol = "Kč"
                },
                true
            );
        }
    }
}