using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Companies;

namespace WebMarketplace.Products;

[AllowAnonymous]
public class ProductBuyerAppService : WebMarketplaceAppService, IProductBuyerAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductManager _productManager;
    private readonly IRepository<Company, Guid> _companyRepository;

    public ProductBuyerAppService(IProductRepository productRepository, ProductManager productManager,
        IRepository<Company, Guid> companyRepository)
    {
        _productRepository = productRepository;
        _productManager = productManager;
        _companyRepository = companyRepository;
    }

    #region ProductDetails

    public async Task<ProductDetailDto> GetProductDetailAsync(Guid id)
    {
        var item = await _productRepository.GetWithCompanyAsync(id);
        var productDetailDto = ObjectMapper.Map<Product, ProductDetailDto>(item.Product);
        productDetailDto.CompanyName = item.Company.DisplayName;
        return productDetailDto;
    }

    #endregion

    #region ProductCards

    public async Task<PagedResultDto<ProductCardDto>> GetProductCardListAsync(ProductCardListFilterDto input)
    {
        var totalCount = await _productRepository.GetFilteredCountAsync(
            input.CompanyId,
            input.Name,
            input.ProductCategory,
            input.ProductType,
            input.MinRating,
            input.MaxRating,
            input.MinPriceAmount,
            input.MaxPriceAmount,
            input.PriceCurrency
        );

        var items = await _productRepository.GetFilteredListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.CompanyId,
            input.Name,
            input.ProductCategory,
            input.ProductType,
            input.MinRating,
            input.MaxRating,
            input.MinPriceAmount,
            input.MaxPriceAmount,
            input.PriceCurrency
        );

        var dtos = items.Select(x =>
        {
            var dto = ObjectMapper.Map<Product, ProductCardDto>(x);
            return dto;
        }).ToList();
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
        var totalCount = await _productRepository.GetReviewWithAuthorCountAsync(
            input.ProductId,
            input.MinRating,
            input.MaxRating);

        var items = await _productRepository.GetReviewWithAuthorListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.ProductId,
            input.MinRating,
            input.MaxRating);

        var dtos = items.Select(x =>
        {
            var dto = ObjectMapper.Map<ProductReview, ProductReviewDto>(x.Review);
            dto.UserName = x.User.Name ?? x.User.UserName;
            return dto;
        }).ToList();
        return new PagedResultDto<ProductReviewDto>(totalCount, dtos);
    }

    #endregion

    #region Mappers

    private ProductDetailDto Map(Product product, Company company)
    {
        var productDetailDto = new ProductDetailDto();

        productDetailDto.Id = product.Id;
        productDetailDto.CompanyId = product.CompanyId;
        productDetailDto.Name = product.Name;
        productDetailDto.ProductCategory = product.ProductCategory;
        productDetailDto.ProductType = product.ProductType;
        productDetailDto.ShortDescription = product.ShortDescription;
        productDetailDto.FullDescription = product.FullDescription;
        productDetailDto.Rating = product.Rating;

        productDetailDto.CompanyName = company.Name;
        productDetailDto.PriceAmount = product.ProductPrice?.Amount ?? 0;
        productDetailDto.PriceCurrency = product.ProductPrice?.Currency ?? string.Empty;

        return productDetailDto;
    }

    #endregion
}