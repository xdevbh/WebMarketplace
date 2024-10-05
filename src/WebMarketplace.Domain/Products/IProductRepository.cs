using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Products;

public interface IProductRepository : IRepository<Product, Guid>
{
    Task<IQueryable<Product>> GetFilteredQueryableAsync(
        Guid? companyId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null);

    Task<List<Product>> GetFilteredListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? companyId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default);

    Task<long> GetFilteredCountAsync(
        Guid? companyId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default);


    Task<IQueryable<ProductWithCompanyQueryResultItem>> GetWithCompanyQueryableAsync(
        Guid? companyId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string priceCurrency = null);

    Task<List<ProductWithCompanyQueryResultItem>> GetWithCompanyListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? companyId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default);

    Task<long> GetWithCompanyCountAsync(
        Guid? companyId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default);

    Task<ProductWithCompanyQueryResultItem> GetWithCompanyAsync(
        Guid id, 
        CancellationToken cancellationToken = default);

    Task<IQueryable<ProductReview>> GetFilteredReviewQueryableAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null);

    Task<List<ProductReview>> GetFilteredReviewListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default);


    Task<IQueryable<ProductReviewAuthorQueryResultItem>> GetReviewWithAuthorQueryableAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null);
    
    Task<List<ProductReviewAuthorQueryResultItem>> GetReviewWithAuthorListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default);

    Task<long> GetReviewWithAuthorCountAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default);
}