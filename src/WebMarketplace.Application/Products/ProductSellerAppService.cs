using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Users;
using WebMarketplace.Permissions;
using WebMarketplace.Companies;
using WebMarketplace.Companies.Memberships;

namespace WebMarketplace.Products;

[Authorize("SellerOnly")]
public class ProductSellerAppService : WebMarketplaceAppService, IProductSellerAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductManager _productManager;
    private readonly IRepository<Company, Guid> _companyRepository;
    private readonly IRepository<CompanyMembership, Guid> _vendorUserRepository;
    private readonly IBlobContainer _productBlobContainer;

    public ProductSellerAppService(IProductRepository productRepository, ProductManager productManager,
        IRepository<Company, Guid> companyRepository, IRepository<CompanyMembership, Guid> vendorUserRepository,
        IBlobContainer productBlobContainer)
    {
        _productRepository = productRepository;
        _productManager = productManager;
        _companyRepository = companyRepository;
        _vendorUserRepository = vendorUserRepository;
        _productBlobContainer = productBlobContainer;
    }

    #region Product

    public async Task<ProductDto> GetAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        var company = await _companyRepository.GetAsync(product.CompanyId);

        var productDto = ObjectMapper.Map<Product, ProductDto>(product);
        productDto.CompanyName = company.Name;

        return productDto;
    }

    public async Task<PagedResultDto<ProductCardDto>> GetListAsync(ProductCardListFilterDto input)
    {
        var totalCount = await _productRepository.GetProductDetailCountAsync(
            input.CompanyId,
            false,
            input.Name,
            input.ProductCategory,
            input.ProductType,
            input.MinRating,
            input.MaxRating,
            input.MinPriceAmount,
            input.MaxPriceAmount,
            input.PriceCurrency
        );

        var items = await _productRepository.GetProductDetailListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.CompanyId,
            false,
            input.Name,
            input.ProductCategory,
            input.ProductType,
            input.MinRating,
            input.MaxRating,
            input.MinPriceAmount,
            input.MaxPriceAmount,
            input.PriceCurrency
        );

        var dtos = new List<ProductCardDto>();
        foreach (var item in items)
        {
            var dto = ObjectMapper.Map<ProductDetailQueryRequestItem, ProductCardDto>(item);
            dto.PriceAmount = item.CurrentPrice.Amount;
            dto.PriceCurrency = item.CurrentPrice.Currency;
            dtos.Add(dto);
        }

        return new PagedResultDto<ProductCardDto>(totalCount, dtos);
    }

    public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var user = CurrentUser.GetId();
        var vendorUser = await _vendorUserRepository.GetAsync(x => x.UserId == user); // can be null

        if (vendorUser == null)
        {
            throw new AbpAuthorizationException();
        }

        var product = await _productManager.CreateAsync(
            vendorUser.CompanyId,
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
        var vendorUser = await _vendorUserRepository.GetAsync(x => x.UserId == userId);
        var product = await _productRepository.GetAsync(id);

        if (vendorUser == null || product.CompanyId != vendorUser.CompanyId)
        {
            throw new AbpAuthorizationException();
        }

        await _productRepository.DeleteAsync(id);
    }

    #endregion

    #region Publish

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

    #endregion

    #region Prices

    public async Task<ProductImageDto> GetDefaultImageAsync(Guid productId)
    {
        var product = await _productRepository.GetAsync(productId);

        if (product != null || product.DefaultImage == null)
        {
            return null;
        }

        var blob = await _productBlobContainer.GetAllBytesOrNullAsync(product.DefaultImage.BlobName);
        var dto = ObjectMapper.Map<ProductImage, ProductImageDto>(product.DefaultImage);
        dto.Content = blob;
        return dto;
    }

    public async Task<ListResultDto<ProductImageDto>> GetAllImagesAsync(Guid productId)
    {
        var product = await _productRepository.GetAsync(productId);

        if (product == null || product.Images == null || !product.Images.Any())
        {
            return new ListResultDto<ProductImageDto>();
        }

        var imageDtos = new List<ProductImageDto>();

        foreach (var image in product.Images)
        {
            var blob = await _productBlobContainer.GetAllBytesOrNullAsync(image.BlobName);
            var dto = ObjectMapper.Map<ProductImage, ProductImageDto>(image);
            dto.Content = blob;
            imageDtos.Add(dto);
        }

        return new ListResultDto<ProductImageDto>(imageDtos);
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

    #endregion

    #region Images

    public async Task CreateProductImageAsync(CreateProductImageDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }

        var blobName = GuidGenerator.Create().ToString();
        product.AddImage(blobName, input.IsDefault);

        await _productBlobContainer.SaveAsync(blobName, input.Content);
    }

    public async Task DeleteProductImageAsync(DeleteProductImageDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }

        product.RemoveImage(input.BlobName);
        await _productBlobContainer.DeleteAsync(input.BlobName);
    }

    #endregion

    #region Helpers

    private async Task<bool> UserHasAccessToProduct(Product product)
    {
        var userId = CurrentUser.GetId();
        var vendorUser = await _vendorUserRepository.FindAsync(x => x.UserId == userId);

        if (vendorUser != null && product.CompanyId == vendorUser.CompanyId)
        {
            return true;
        }

        return false;
    }

    #endregion
}