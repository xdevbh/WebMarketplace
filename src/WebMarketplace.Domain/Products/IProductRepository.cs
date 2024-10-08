using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Products;

public interface IProductRepository : IRepository<Product, Guid>
{
    // ProductDetail
    
    Task<ProductDetailQueryRequestItem> GetProductDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<IQueryable<ProductDetailQueryRequestItem>> GetProductDetailQueryableAsync(
        Guid? companyId = null,
        bool? isPublished = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string priceCurrency = null);

    Task<List<ProductDetailQueryRequestItem>> GetProductDetailListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? companyId = null,
        bool? isPublished = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default);

    Task<long> GetProductDetailCountAsync(
        Guid? companyId = null,
        bool? isPublished = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default);
    

    // ReviewDetail
    Task<ProductReviewDetailQueryResultItem> GetReviewDetailAsync(
        Guid id ,
        CancellationToken cancellationToken = default);

    Task<IQueryable<ProductReviewDetailQueryResultItem>> GetReviewDetailQueryableAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null);

    Task<List<ProductReviewDetailQueryResultItem>> GetReviewDetailListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default);

    Task<long> GetReviewDetailCountAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default);
}