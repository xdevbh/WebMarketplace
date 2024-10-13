using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Users;
using WebMarketplace.Permissions;
using WebMarketplace.Companies;
using WebMarketplace.Companies.Memberships;
using Volo.Abp.ObjectMapping;
using System.Reflection.Metadata.Ecma335;
using System.Net.Mime;
using System.IO;

namespace WebMarketplace.Products;

[Authorize("SellerOnly")]
public class ProductSellerAppService : WebMarketplaceAppService, IProductSellerAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductManager _productManager;
    private readonly IRepository<Company, Guid> _companyRepository;
    private readonly IRepository<CompanyMembership, Guid> _companyMembershipRepository;
    private readonly IBlobContainer<ProductImageContainer> _productBlobContainer;

    public ProductSellerAppService(
        IProductRepository productRepository, 
        ProductManager productManager,
        IRepository<Company, Guid> companyRepository, 
        IRepository<CompanyMembership, Guid> companyMembershipRepository,
        IBlobContainer<ProductImageContainer> productBlobContainer)
    {
        _productRepository = productRepository;
        _productManager = productManager;
        _companyRepository = companyRepository;
        _companyMembershipRepository = companyMembershipRepository;
        _productBlobContainer = productBlobContainer;
    }

    #region Product

    public async Task<ProductDto> GetAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        var company = await _companyRepository.GetAsync(product.CompanyId);

        var productDto = ObjectMapper.Map<Product, ProductDto>(product);
        return productDto;
    }

    public async Task<PagedResultDto<ProductListItemDto>> GetListAsync(ProductListFilterDto input)
    {
        var user = CurrentUser.GetId();
        var companyMembership = await _companyMembershipRepository.GetAsync(x => x.UserId == user);
        if (companyMembership == null)
        {
            throw new AbpAuthorizationException();
        }

        var totalCount = await _productRepository.GetProductDetailCountAsync(
            companyMembership.CompanyId,
            input.IsPublished,
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
            companyMembership.CompanyId,
            input.IsPublished,
            input.Name,
            input.ProductCategory,
            input.ProductType,
            input.MinRating,
            input.MaxRating,
            input.MinPriceAmount,
            input.MaxPriceAmount,
            input.PriceCurrency
        );

        var dtos = new List<ProductListItemDto>();
        foreach (var item in items)
        {
            var dto = ObjectMapper.Map<ProductDetailQueryRequestItem, ProductListItemDto>(item);
            dto.PriceAmount = item.PriceAmount;
            dto.PriceCurrency = item.PriceCurrency;
            dtos.Add(dto);
        }

        return new PagedResultDto<ProductListItemDto>(totalCount, dtos);
    }

    public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var user = CurrentUser.GetId();
        var companyMembership = await _companyMembershipRepository.GetAsync(x => x.UserId == user); // can be null
        if (companyMembership == null)
        {
            throw new AbpAuthorizationException();
        }


        var product = await _productManager.CreateAsync(
            companyMembership.CompanyId,
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
        var companyMembership = await _companyMembershipRepository.GetAsync(x => x.UserId == userId);
        var product = await _productRepository.GetAsync(id);

        if (companyMembership == null || product.CompanyId != companyMembership.CompanyId)
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

    public async Task AddProductPriceAsync(CreateUpdateProductPriceDto input)
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

    public async Task<PagedResultDto<ProductPriceDto>> GetPricesAsync(ProductPriceListFilterDto input)
    {
        var totalCount = await _productRepository.GetPriceCountAsync(input.ProductId);

        var prices = await _productRepository.GetPriceListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.ProductId);

        var dtos = ObjectMapper.Map<List<ProductPrice>, List<ProductPriceDto>>(prices);
        return new PagedResultDto<ProductPriceDto>(totalCount, dtos);
    }

    public async Task DeleteProductPriceAsync(DeleteProductPriceDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }

        product.RemovePrice(input.Date.DateTime);
    }

    #endregion

    #region Images

    public async Task<ProductImageDto> GetDefaultImageAsync(Guid productId)
    {
        var product = await _productRepository.GetAsync(productId);

        if (product == null || product.DefaultImage == null)
        {
            return null;
        }
        var dto = ObjectMapper.Map<ProductImage, ProductImageDto>(product.DefaultImage);

        //var blob = await _productBlobContainer.GetAllBytesOrNullAsync(product.DefaultImage.BlobName);
        //dto.Content = blob;

        var bytes = await _productBlobContainer.GetAllBytesOrNullAsync(product.DefaultImage.BlobName);
        dto.Content = bytes;

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
            var dto = ObjectMapper.Map<ProductImage, ProductImageDto>(image);

            //var blob = await _productBlobContainer.GetAllBytesOrNullAsync(image.BlobName);
            //dto.Content = blob;

            var bytes = await _productBlobContainer.GetAllBytesOrNullAsync(image.BlobName);
            dto.Content = bytes;

            imageDtos.Add(dto);
        }

        return new ListResultDto<ProductImageDto>(imageDtos);
    }

    private async Task SaveCoverImageAsync(Guid id, IRemoteStreamContent coverImageContent)
    {
        var blobName = id.ToString();

        await _productBlobContainer.SaveAsync(blobName, coverImageContent.GetStream(), overrideExisting: true);
    }

    public async Task AddProductImageAsync(CreateProductImageDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }

        var blobName = GuidGenerator.Create().ToString() + "_" + input.FileName;

        if (input.Content == null || input.Content.Length <= 0)
        {
            return; // todo: throw exception
        }

        await _productBlobContainer.SaveAsync(blobName, input.Content, overrideExisting: true);

        product.AddImage(blobName, input.IsDefault);
    }

    public async Task SetDefaultImageAsync(UpdateProductImageDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);

        var hasAccess = await UserHasAccessToProduct(product);
        if (!hasAccess)
        {
            throw new AbpAuthorizationException();
        }
        
        product.SetDefaultImage(input.BlobName, input.IsDefault);
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
        var companyMembership = await _companyMembershipRepository.FindAsync(x => x.UserId == userId);

        if (companyMembership != null && product.CompanyId == companyMembership.CompanyId)
        {
            return true;
        }

        return false;
    }

    #endregion
}