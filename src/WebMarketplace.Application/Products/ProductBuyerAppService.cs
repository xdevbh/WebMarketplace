using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using WebMarketplace.Companies;

namespace WebMarketplace.Products;

[AllowAnonymous]
public class ProductBuyerAppService : WebMarketplaceAppService, IProductBuyerAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductManager _productManager;
    private readonly IRepository<Company, Guid> _companyRepository;
    private readonly IBlobContainer<ProductImageContainer> _productBlobContainer;


    public ProductBuyerAppService(IProductRepository productRepository, ProductManager productManager, IRepository<Company, Guid> companyRepository, IBlobContainer<ProductImageContainer> productBlobContainer)
    {
        _productRepository = productRepository;
        _productManager = productManager;
        _companyRepository = companyRepository;
        _productBlobContainer = productBlobContainer;
    }

    #region Products

    public async Task<ProductDetailDto> GetProductDetailAsync(Guid id)
    {
        var item = await _productRepository.GetProductDetailAsync(id);
        var dto = ObjectMapper.Map<ProductDetailQueryRequestItem, ProductDetailDto>(item);
        dto.PriceAmount = item.CurrentPrice.Amount;
        dto.PriceCurrency = item.CurrentPrice.Currency;
        return dto;
    }

    public async Task<PagedResultDto<ProductCardDto>> GetProductCardListAsync(ProductCardListFilterDto input)
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

    #endregion

    #region Reviews

    public async Task CreateReview(CreateUpdateProductReviewDto input) // todo: if product is in user orders
    {
        var product = await _productRepository.GetAsync(input.ProductId);
        await _productManager.AddProductReviewAsync(product, input.UserId, input.Rating, input.Comment);
    }

    public async Task<PagedResultDto<ProductReviewDto>> GetReviewListAsync(ProductReviewListFilterDto input)
    {
        var totalCount = await _productRepository.GetReviewDetailCountAsync(
            input.ProductId,
            input.MinRating,
            input.MaxRating);

        var items = await _productRepository.GetReviewDetailListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.ProductId,
            input.MinRating,
            input.MaxRating);

        var dtos = ObjectMapper.Map<List<ProductReviewDetailQueryResultItem>, List<ProductReviewDto>>(items);
        return new PagedResultDto<ProductReviewDto>(totalCount, dtos);
    }

    #endregion

    #region Images

    public async Task<IRemoteStreamContent> GetDefaultImageAsync(Guid productId)
    {
        var product = await _productRepository.GetAsync(productId);
        if (product != null || product.DefaultImage == null)
        {
            return null; 
        }
        
        var imageContent = await _productBlobContainer.GetOrNullAsync(product.DefaultImage.BlobName);
        if (imageContent == null)
        {
            return null;
        }

        return new RemoteStreamContent(imageContent);
    }

    public async Task<ListResultDto<IRemoteStreamContent>> GetAllImagesAsync(Guid productId)
    {
        var product = await _productRepository.GetAsync(productId);

        if (product == null || product.Images == null || !product.Images.Any())
        {
            return new ListResultDto<IRemoteStreamContent>();
        }

        var imageContentList = new List<IRemoteStreamContent>();
        foreach (var image in product.Images)
        {
            var imageContent = await _productBlobContainer.GetOrNullAsync(image.BlobName);
            var tt = new RemoteStreamContent(imageContent);
            imageContentList.Add(tt);
        }

        return new ListResultDto<IRemoteStreamContent>(imageContentList);
    }
    
    #endregion

    #region Mappers

    private async Task<ProductDto> Map(ProductDetailQueryRequestItem item)
    {
        var productDto = new ProductDto();
        productDto = ObjectMapper.Map<ProductDetailQueryRequestItem, ProductDto>(item);
        productDto.PriceAmount = item.CurrentPrice.Amount;
        productDto.PriceCurrency = item.CurrentPrice.Currency;
        
        // var reviews = await GetReviewListAsync(new ProductReviewListFilterDto(item.Id));
        // productDto.Reviews = reviews;
        //
        // var images = await GetAllImagesAsync(item.Id);
        // productDto.Images = images;
        
        return productDto;
    }

    #endregion
}