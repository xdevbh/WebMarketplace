using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using WebMarketplace.Products;
using WebMarketplace.Vendors;

namespace WebMarketplace.EntityFrameworkCore.Products;

public class ProductRepository : EfCoreRepository<WebMarketplaceDbContext, Product, Guid>, IProductRepository
{
    public ProductRepository(IDbContextProvider<WebMarketplaceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Product>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    #region Products

    public async Task<IQueryable<Product>> GetFilteredQueryableAsync(
        Guid? vendorId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null)
    {
        var dbContext = await GetDbContextAsync();

        var query = await GetQueryableAsync();
        query.WhereIf(vendorId != null, x => x.VendorId == vendorId.Value);
        query.WhereIf(!name.IsNullOrEmpty(), x => x.Name.Contains(name));
        query.WhereIf(productCategory != null, x => x.ProductCategory == productCategory);
        query.WhereIf(productType != null, x => x.ProductType == productType);
        query.WhereIf(minRating != null, x => x.Rating >= minRating);
        query.WhereIf(maxRating != null, x => x.Rating <= maxRating);
        query.WhereIf(minPriceAmount != null, x => x.ProductPrice != null && x.ProductPrice.Amount >= minPriceAmount);
        query.WhereIf(maxPriceAmount != null, x => x.ProductPrice != null && x.ProductPrice.Amount <= maxPriceAmount);
        query.WhereIf(priceCurrency != null, x => x.ProductPrice != null && x.ProductPrice.Currency == priceCurrency);

        var tt = (await GetQueryableAsync()).IncludeDetails();
        return tt;
    }

    public async Task<List<Product>> GetFilteredListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? vendorId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = await GetFilteredQueryableAsync(
            vendorId,
            name,
            productCategory,
            productType,
            minRating,
            maxRating,
            minPriceAmount,
            maxPriceAmount,
            priceCurrency);

        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(Product.CreationTime) + " DESC";
        }
        else
        {
            sorting = nameof(Product) + "." + sorting;
        }

        query = query.OrderBy(sorting);
        query = query.PageBy(skipCount, maxResultCount);
        var list = await query.ToListAsync(GetCancellationToken(cancellationToken));

        return list;
    }

    public async Task<long> GetFilteredCountAsync(
        Guid? vendorId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = await GetFilteredQueryableAsync(
            vendorId,
            name,
            productCategory,
            productType,
            minRating,
            maxRating,
            minPriceAmount,
            maxPriceAmount,
            priceCurrency);

        var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }

    #endregion 

    #region ProductVendorQueryResultItems

    public async Task<IQueryable<ProductVendorQueryResultItem>> GetWithVendorQueryableAsync(
        Guid? vendorId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null)
    {
        var dbContext = await GetDbContextAsync();

        var query =
            from product in dbContext.Set<Product>().IncludeDetails()
            join vendor in dbContext.Set<Vendor>() on product.VendorId equals vendor.Id
            select new ProductVendorQueryResultItem(product, vendor);

        query.WhereIf(vendorId != null, x => vendorId != null && x.Product.VendorId == vendorId.Value);
        query.WhereIf(!name.IsNullOrEmpty(), x => x.Product.Name.Contains(name));
        query.WhereIf(productCategory != null, x => x.Product.ProductCategory == productCategory);
        query.WhereIf(productType != null, x => x.Product.ProductType == productType);
        query.WhereIf(minRating != null, x => x.Product.Rating >= minRating);
        query.WhereIf(maxRating != null, x => x.Product.Rating <= maxRating);
        query.WhereIf(minPriceAmount != null, x => x.Product.ProductPrice != null && x.Product.ProductPrice.Amount >= minPriceAmount);
        query.WhereIf(maxPriceAmount != null, x => x.Product.ProductPrice != null && x.Product.ProductPrice.Amount <= maxPriceAmount);
        query.WhereIf(priceCurrency != null, x => x.Product.ProductPrice != null && x.Product.ProductPrice.Currency == priceCurrency);

        return query;
    }

    public async Task<List<ProductVendorQueryResultItem>> GetWithVendorListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? vendorId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetWithVendorQueryableAsync(
            vendorId,
            name,
            productCategory,
            productType,
            minRating, maxRating,
            minPriceAmount,
            maxPriceAmount,
            priceCurrency);

        //// todo: fix sorting
        //if (sorting.IsNullOrWhiteSpace())
        //{
        //    sorting = "product.creationTime desc";
        //}
        //else
        //{
        //    sorting = nameof(Product) + "." + sorting;
        //}

        query = query.OrderBy(sorting);
        query = query.PageBy(skipCount, maxResultCount);
        var list = await query.ToListAsync(GetCancellationToken(cancellationToken));
        return list;
    }

    public async Task<long> GetWithVendorCountAsync(
        Guid? vendorId = null,
        string? name = null,
        ProductCategory? productCategory = null,
        ProductType? productType = null,
        double? minRating = null,
        double? maxRating = null,
        decimal? minPriceAmount = null,
        decimal? maxPriceAmount = null,
        string? priceCurrency = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetWithVendorQueryableAsync(
            vendorId,
            name,
            productCategory,
            productType,
            minRating, maxRating,
            minPriceAmount,
            maxPriceAmount,
            priceCurrency);

        var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }

    public async Task<ProductVendorQueryResultItem> GetWithVendorAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query =
            from product in dbContext.Set<Product>().IncludeDetails()
            join vendor in dbContext.Set<Vendor>() on product.VendorId equals vendor.Id
            where product.Id == id
            select new ProductVendorQueryResultItem(product, vendor);

        var item = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return item;
    }

    #endregion

    #region Reviews

    public async Task<IQueryable<ProductReview>> GetFilteredReviewQueryableAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null)
    {
        var dbContext = await GetDbContextAsync();
        var query = dbContext.Set<ProductReview>()
            .WhereIf(productId != null, x => x.ProductId == productId)
            .WhereIf(minRating != null, x => x.Rating >= minRating)
            .WhereIf(maxRating != null, x => x.Rating <= maxRating);

        return query;
    }

    public async Task<List<ProductReview>> GetFilteredReviewListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetFilteredReviewQueryableAsync(
            productId,
            minRating,
            maxRating);

        return await query
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(ProductReview.CreationTime) + " DESC" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<IQueryable<ProductReviewAuthorQueryResultItem>> GetReviewWithAuthorQueryableAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null)
    {
        var dbContext = await GetDbContextAsync();

        var query =
            from review in dbContext.Set<ProductReview>()
            join user in dbContext.Set<IdentityUser>() on review.UserId equals user.Id
            select new ProductReviewAuthorQueryResultItem(review, user);

        query = query
            .WhereIf(productId.HasValue, x => x.Review.ProductId == productId.Value)
            .WhereIf(minRating.HasValue, x => x.Review.Rating >= minRating.Value)
            .WhereIf(maxRating.HasValue, x => x.Review.Rating <= maxRating.Value);

        return query;
    }

    public async Task<List<ProductReviewAuthorQueryResultItem>> GetReviewWithAuthorListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetReviewWithAuthorQueryableAsync(productId, minRating, maxRating);

        // if (!string.IsNullOrWhiteSpace(sorting))
        // {
        //     query = query.OrderBy(sorting); 
        // }
        // else
        // {
        //     query = query.OrderByDescending(x => x.Review.CreationTime);  
        // }

        query = query.PageBy(skipCount, maxResultCount);

        var list = await query.ToListAsync(cancellationToken);
        return list;
    }


    public async Task<long> GetReviewWithAuthorCountAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetReviewWithAuthorQueryableAsync(
            productId,
            minRating,
            maxRating);

        var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }

    #endregion
}