using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using WebMarketplace.Companies;

namespace WebMarketplace.Products;

[Authorize]
public class ProductBuyerAppService : WebMarketplaceAppService, IProductBuyerAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductManager _productManager;
    private readonly IRepository<Company, Guid> _companyRepository;
    private readonly IBlobContainer<ProductImageContainer> _productBlobContainer;

    public ProductBuyerAppService(IProductRepository productRepository, ProductManager productManager,
        IRepository<Company, Guid> companyRepository, IBlobContainer<ProductImageContainer> productBlobContainer)
    {
        _productRepository = productRepository;
        _productManager = productManager;
        _companyRepository = companyRepository;
        _productBlobContainer = productBlobContainer;
    }

    #region Products

    [AllowAnonymous]
    public async Task<ProductDetailDto> GetProductDetailAsync(Guid id)
    {
        var item = await _productRepository.GetProductDetailAsync(id);
        var dto = ObjectMapper.Map<ProductDetailQueryRequestItem, ProductDetailDto>(item);
        dto.PriceAmount = item.PriceAmount;
        dto.PriceCurrency = item.PriceCurrency;
        return dto;
    }

    [AllowAnonymous]
    public async Task<ProductCardListResultDto> GetProductCardListAsync(ProductCardListFilterDto input)
    {
        var sorting = string.Empty;
        switch (input.Sorting)
        {
            case ProductSorting.Popularity:
                sorting = "name ASC"; // todo: order count
                break;
            case ProductSorting.RatingAsc:
                sorting = "rating ASC";
                break;
            case ProductSorting.RatingDesc:
                sorting = "rating DESC";
                break;
            case ProductSorting.PriceAsc:
                sorting = "PriceAmount ASC";
                break;
            case ProductSorting.PriceDesc:
                sorting = "PriceAmount DESC";
                break;
        }

        var totalCount = await _productRepository.GetProductDetailCountAsync(
            input.CompanyId,
            true,
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
            sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.CompanyId,
            true,
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
            dto.PriceAmount = item.PriceAmount;
            dto.PriceCurrency = item.PriceCurrency;
            if (!item.DefaultImageBlobName.IsNullOrWhiteSpace())
            {
                dto.ImageContent = await _productBlobContainer.GetAllBytesOrNullAsync(item.DefaultImageBlobName);
            }

            dtos.Add(dto);
        }

        var minPrice = await _productRepository.GetMinPriceAsync();
        var maxPrice = await _productRepository.GetMaxPriceAsync();
        var minRating = await _productRepository.GetMinRatingAsync();
        var maxRating = await _productRepository.GetMaxRatingAsync();

        var result = new ProductCardListResultDto()
        {
            TotalCount = totalCount,
            Items = dtos,
            MinPriceAmount = input.MinPriceAmount ?? minPrice,
            MaxPriceAmount = input.MaxPriceAmount ?? maxPrice,
            MinRating = input.MinRating ?? minRating,
            MaxRating = input.MaxRating ?? maxRating,
            PriceCurrency = input.PriceCurrency ?? "CZK"
        };

        return result;
    }

    [AllowAnonymous]
    public async Task<PagedResultDto<ProductCardDto>> GetSimilarProductCardListAsync(SimilarProductCardListFilterDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);

        var query = await _productRepository.GetProductDetailQueryableAsync(null, true);
        query = query.Where(x => x.Id != input.ProductId);
        query = query.Where(x => x.ProductCategory == product.ProductCategory);
        query = query.Where(x => x.ProductType == product.ProductType);

        var totalCount = await AsyncExecuter.CountAsync(query);
        query = query
            .OrderBy(x => x.Rating)
            .PageBy(input.SkipCount, input.MaxResultCount);

        var items = await AsyncExecuter.ToListAsync(query);
        var dtos = new List<ProductCardDto>();
        foreach (var item in items)
        {
            var dto = ObjectMapper.Map<ProductDetailQueryRequestItem, ProductCardDto>(item);
            dto.PriceAmount = item.PriceAmount;
            dto.PriceCurrency = item.PriceCurrency;
            if (!item.DefaultImageBlobName.IsNullOrWhiteSpace())
            {
                dto.ImageContent = await _productBlobContainer.GetAllBytesOrNullAsync(item.DefaultImageBlobName);
            }

            dtos.Add(dto);
        }

        return new PagedResultDto<ProductCardDto>(totalCount, dtos);
    }

    #endregion

    #region Reviews

    [Authorize]
    public async Task CreateReview(CreateUpdateProductReviewBuyerDto input) // todo: if product is in user orders
    {
        var product = await _productRepository.GetAsync(input.ProductId);
        var userId = CurrentUser.GetId();

        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }

        if (product.Reviews.Any(x => x.UserId == userId))
        {
            throw new BusinessException(
                L[WebMarketplaceDomainErrorCodes.UserHasProductReview]);
        }

        var existedReview = await _productRepository.GetPriceQueryableAsync();

        await _productManager.AddProductReviewAsync(product, userId, input.Rating, input.Comment);
    }

    [AllowAnonymous]
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

    public async Task<bool> HasReviewAsync(Guid productId)
    {
        var userId = CurrentUser.GetId();
        var product = await _productRepository.GetAsync(productId);
        
        if (product.Reviews.Any(x => x.UserId == userId))
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Images

    [AllowAnonymous]
    public async Task<ProductImageDto> GetDefaultImageAsync(Guid productId)
    {
        var product = await _productRepository.GetAsync(productId);

        if (product == null || product.Images == null || !product.Images.Any() || product.DefaultImage != null)
        {
            return new ProductImageDto();
        }

        var dto = ObjectMapper.Map<ProductImage, ProductImageDto>(product.DefaultImage);
        var bytes = await _productBlobContainer.GetAllBytesOrNullAsync(product.DefaultImage.BlobName);
        dto.Content = bytes;

        return dto;
    }

    [AllowAnonymous]
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


            var bytes = await _productBlobContainer.GetAllBytesOrNullAsync(image.BlobName);
            dto.Content = bytes;

            imageDtos.Add(dto);
        }

        return new ListResultDto<ProductImageDto>(imageDtos);
    }

    #endregion

    #region Mappers

    private async Task<ProductDto> Map(ProductDetailQueryRequestItem item)
    {
        var productDto = new ProductDto();
        productDto = ObjectMapper.Map<ProductDetailQueryRequestItem, ProductDto>(item);

        // var reviews = await GetReviewListAsync(new ProductReviewListFilterDto(item.Id));
        // productDto.Reviews = reviews;
        //
        // var images = await GetAllImagesAsync(item.Id);
        // productDto.Images = images;

        return productDto;
    }

    #endregion
}