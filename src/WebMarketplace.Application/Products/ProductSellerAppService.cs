using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Users;
using WebMarketplace.Permissions;
using WebMarketplace.Vendors;
using WebMarketplace.Vendors.VendorUsers;

namespace WebMarketplace.Products;

[Authorize("SellerOnly")]
public class ProductSellerAppService : WebMarketplaceAppService, IProductSellerAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductManager _productManager;
    private readonly IRepository<Vendor, Guid> _vendorRepository;
    private readonly IRepository<VendorUser, Guid> _vendorUserRepository;

    public ProductSellerAppService(IProductRepository productRepository, ProductManager productManager, IRepository<Vendor, Guid> vendorRepository, IRepository<VendorUser, Guid> vendorUserRepository)
    {
        _productRepository = productRepository;
        _productManager = productManager;
        _vendorRepository = vendorRepository;
        _vendorUserRepository = vendorUserRepository;
    }

    public async Task<ProductDto> GetAsync(Guid id)
    {
       var product = await _productRepository.GetAsync(id);
       var vendor = await _vendorRepository.GetAsync(product.VendorId);
       
       var productDto = ObjectMapper.Map<Product, ProductDto>(product);
       productDto.VendorName = vendor.Name;
       
       return productDto;
    }

    public async Task<List<ProductDto>> GetListAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var user = CurrentUser.GetId();
        var vendorUser = await _vendorUserRepository.GetAsync(x=>x.UserId == user); // can be null

        if (vendorUser == null)
        {
            throw new AbpAuthorizationException();
        }
        
        var product = await _productManager.CreateAsync(
            vendorUser.VendorId,
            input.Name,
            input.ProductCategory,
            input.ProductType,
            input.ShortDescription,
            input.FullDescription);

        await _productRepository.InsertAsync(product);
        var dto = ObjectMapper.Map<Product, ProductDto>(product);
        return dto;
    }

    public async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var product = await _productRepository.GetAsync(id);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }
            
        await _productManager.EditAsync(
            product, 
            input.Name, 
            input.ProductCategory,
            input.ProductType, 
            input.ShortDescription, 
            input.FullDescription);

        await _productRepository.UpdateAsync(product);
        var dto = ObjectMapper.Map<Product, ProductDto>(product);
        return dto;
    }

    public async Task DeleteAsync(Guid id)
    {
        var userId = CurrentUser.GetId();
        var vendorUser = await _vendorUserRepository.GetAsync(x=>x.UserId == userId);
        var product = await _productRepository.GetAsync(id);
        
        if (vendorUser == null || product.VendorId != vendorUser.VendorId)
        {
            throw new AbpAuthorizationException();
        }
        
        await _productRepository.DeleteAsync(id);
    }

    [Authorize(WebMarketplacePermissions.Products.Publish)]
    public async Task PublishAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }
        
        product.Publish(true);
    }

    [Authorize(WebMarketplacePermissions.Products.Publish)]
    public async Task UnpublishAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }    
        
        product.Publish(false);
    }

    public async Task CreateProductPriceAsync(CreateUpdateProductPriceDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }
        
        await _productManager.AddProductPriceAsync(
            product,
            input.Date,
            input.Amount,
            input.Currency);
    }

    private async Task<bool> UserHasAccessToProduct(Product product)
    {
        var userId = CurrentUser.GetId();
        var vendorUser = await _vendorUserRepository.FindAsync(x=>x.UserId == userId);
    
        if (vendorUser != null && product.VendorId == vendorUser.VendorId)
        {
            return true;
        }
    
        return false;
    }
}