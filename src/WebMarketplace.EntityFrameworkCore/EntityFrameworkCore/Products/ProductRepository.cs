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
using WebMarketplace.Companies;

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
    
    
    #region ProductCompanyQueryResultItems

    public async Task<ProductDetailQueryRequestItem> GetProductDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = await GetProductDetailQueryableAsync();
        query = query.Where(x => x.Id == id);
        var item = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return item;
    }

    public async Task<IQueryable<ProductDetailQueryRequestItem>> GetProductDetailQueryableAsync(
        Guid? companyId = null,
        bool? isPublished = null,
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
            join company in dbContext.Set<Company>() on product.CompanyId equals company.Id
            select new ProductDetailQueryRequestItem
            {
                Id = product.Id,
                CompanyId = product.CompanyId,
                CompanyName = company.Name,
                Name = product.Name,
                ProductCategory = product.ProductCategory,
                ProductType = product.ProductType,
                ShortDescription = product.ShortDescription,
                FullDescription = product.FullDescription,
                IsPublished = product.IsPublished,
                Rating = product.Rating,
                CurrentPrice = product.CurrentPrice,
                CreationTime = product.CreationTime
            };
            

        query.WhereIf(companyId != null, x => companyId != null && x.CompanyId == companyId.Value);
        query.WhereIf(isPublished != null, x => isPublished != null && x.IsPublished == isPublished.Value);
        query.WhereIf(!name.IsNullOrEmpty(), x => x.Name.Contains(name));
        query.WhereIf(productCategory != null, x => x.ProductCategory == productCategory);
        query.WhereIf(productType != null, x => x.ProductType == productType);
        query.WhereIf(minRating != null, x => x.Rating >= minRating);
        query.WhereIf(maxRating != null, x => x.Rating <= maxRating);
        query.WhereIf(minPriceAmount != null,
            x => x.CurrentPrice != null && x.CurrentPrice.Amount >= minPriceAmount);
        query.WhereIf(maxPriceAmount != null,
            x => x.CurrentPrice != null && x.CurrentPrice.Amount <= maxPriceAmount);
        query.WhereIf(priceCurrency != null,
            x => x.CurrentPrice != null && x.CurrentPrice.Currency == priceCurrency);

        return query;
    }

    public async Task<List<ProductDetailQueryRequestItem>> GetProductDetailListAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = await GetProductDetailQueryableAsync(
            companyId,
            isPublished,
            name,
            productCategory,
            productType,
            minRating, maxRating,
            minPriceAmount,
            maxPriceAmount,
            priceCurrency);

        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = "CreationTime DESC";
        }

        query = query.OrderBy(sorting);
        query = query.PageBy(skipCount, maxResultCount);
        var list = await query.ToListAsync(GetCancellationToken(cancellationToken));
        return list;
    }

    public async Task<long> GetProductDetailCountAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = await GetProductDetailQueryableAsync(
            companyId,
            isPublished,
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

    #endregion

    #region Reviews

    public async Task<ProductReviewDetailQueryResultItem> GetReviewDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query =
            from review in dbContext.Set<ProductReview>()
            join user in dbContext.Set<IdentityUser>() on review.UserId equals user.Id
            join product in dbContext.Set<Product>() on review.ProductId equals product.Id
            where product.Id == id
            select new ProductReviewDetailQueryResultItem
            {
                UserId = user.Id,
                UserName = user.Name ?? user.UserName,
                ProductId = product.Id,
                ProductName = product.Name,
                Rating = review.Rating,
                Comment = review.Comment,
                CreationTime = review.CreationTime
            };
        
        var item = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return item;
    }

    public async Task<IQueryable<ProductReviewDetailQueryResultItem>> GetReviewDetailQueryableAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null)
    {
        var dbContext = await GetDbContextAsync();

        var query =
            from review in dbContext.Set<ProductReview>()
            join user in dbContext.Set<IdentityUser>() on review.UserId equals user.Id
            join product in dbContext.Set<Product>() on review.ProductId equals product.Id
            select new ProductReviewDetailQueryResultItem
            {
                UserId = user.Id,
                UserName = user.Name ?? user.UserName,
                ProductId = product.Id,
                ProductName = product.Name,
                Rating = review.Rating,
                Comment = review.Comment,
                CreationTime = review.CreationTime
            };

        query = query
            .WhereIf(productId.HasValue, x => productId != null && x.ProductId == productId.Value)
            .WhereIf(minRating.HasValue, x => minRating != null && x.Rating >= minRating.Value)
            .WhereIf(maxRating.HasValue, x => maxRating != null && x.Rating <= maxRating.Value);

        return query;
    }

    public async Task<List<ProductReviewDetailQueryResultItem>> GetReviewDetailListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetReviewDetailQueryableAsync(
            productId, 
            minRating, 
            maxRating);

        if (string.IsNullOrWhiteSpace(sorting))
        {
            sorting = "CreationTime DESC";
        }

        query = query.OrderBy(sorting); 
        query = query.PageBy(skipCount, maxResultCount);

        var list = await query.ToListAsync(cancellationToken);
        return list;
    }


    public async Task<long> GetReviewDetailCountAsync(
        Guid? productId = null,
        double? minRating = null,
        double? maxRating = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetReviewDetailQueryableAsync(
            productId,
            minRating,
            maxRating);

        var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }

    #endregion
}