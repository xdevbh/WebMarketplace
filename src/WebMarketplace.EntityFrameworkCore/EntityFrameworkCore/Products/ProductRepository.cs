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
            select new
            {
                Product = product,
                CompanyName = company.Name,
                Reviews = product.Reviews,
                Prices = product.Prices,
                Images = product.Images
            }
            into result
            select new ProductDetailQueryRequestItem
            {
                Id = result.Product.Id,
                CompanyId = result.Product.CompanyId,
                CompanyName = result.CompanyName,
                Name = result.Product.Name,
                ProductCategory = result.Product.ProductCategory,
                ProductType = result.Product.ProductType,
                ShortDescription = result.Product.ShortDescription,
                FullDescription = result.Product.FullDescription,
                IsPublished = result.Product.IsPublished,
                Rating = result.Reviews.Any() ? result.Reviews.Average(r => r.Rating) : 0,
                PriceAmount = result.Prices.Any()
                    ? result.Prices.OrderByDescending(p => p.Date).FirstOrDefault().Amount
                    : 0,
                PriceCurrency = result.Prices.Any()
                    ? result.Prices.OrderByDescending(p => p.Date).FirstOrDefault().Currency
                    : string.Empty,
                PriceDate = result.Prices.Any()
                    ? result.Prices.OrderByDescending(p => p.Date).FirstOrDefault().Date
                    : (DateTime?)null,
                DefaultImageBlobName = result.Product.Images.Any()
                    ? result.Product.Images.FirstOrDefault(x => x.IsDefault) != null
                        ? result.Product.Images.FirstOrDefault(x => x.IsDefault).BlobName
                        : result.Images.FirstOrDefault().BlobName
                    : string.Empty,
                CreationTime = result.Product.CreationTime
            };


        if (companyId != null)
        {
            query = query.Where(x => x.CompanyId == companyId.Value);
        }

        if (isPublished != null)
        {
            query = query.Where(x => x.IsPublished == isPublished.Value);
        }

        if (!name.IsNullOrWhiteSpace())
        {
            query = query.Where(x => x.Name.Contains(name));
        }

        if (productCategory != null)
        {
            query = query.Where(x => x.ProductCategory == productCategory);
        }

        if (productType != null)
        {
            query = query.Where(x => x.ProductType == productType);
        }

        if (minRating != null)
        {
            query = query.Where(x => x.Rating >= minRating);
        }

        if (maxRating != null)
        {
            query = query.Where(x => x.Rating <= maxRating);
        }

        if (minPriceAmount != null)
        {
            query = query.Where(x => x.PriceAmount >= minPriceAmount);
        }

        if (maxPriceAmount != null)
        {
            query = query.Where(x => x.PriceAmount <= maxPriceAmount);
        }

        if (!priceCurrency.IsNullOrWhiteSpace())
        {
            query = query.Where(x => x.PriceCurrency != null && x.PriceCurrency == priceCurrency);
        }


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
            minRating, 
            maxRating,
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

    public async Task<double> GetMinRatingAsync(
        Guid? productId = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
    
        var query =
            from product in dbContext.Set<Product>() 
            join review in dbContext.Set<ProductReview>() 
                on product.Id equals review.ProductId into productReviews 
            from review in productReviews.DefaultIfEmpty() 
            where productId == null ? true : product.Id == productId 
            select review != null ? review.Rating : (double?)null; 
        
        var minRating = await query.MinAsync(x => x ?? 0, GetCancellationToken(cancellationToken));
    
        return minRating;
    }

    
    public async Task<double> GetMaxRatingAsync(
        Guid? productId = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query =
            from product in dbContext.Set<Product>() 
            join review in dbContext.Set<ProductReview>() 
                on product.Id equals review.ProductId into productReviews 
            from review in productReviews.DefaultIfEmpty() 
            where productId == null || product.Id == productId 
            select review != null ? review.Rating : (double?)null; 
        
        var maxRating = await query.MaxAsync(x => x ?? 0, GetCancellationToken(cancellationToken));
        return maxRating;
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

    public async Task<List<ProductReviewDetailQueryResultItem>> GetReviewDetailListAsync(
        string? sorting = null,
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

    #region Price
    public async Task<decimal> GetMinPriceAsync(
        Guid? productId = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query =
            from price in dbContext.Set<ProductPrice>()
            select price;
        
        if (productId != null)
        {
            query = query.Where(x => x.ProductId == productId);
        }
        
        var minPrice = await query.MinAsync(x => x.Amount, GetCancellationToken(cancellationToken));
        return minPrice;
    }
    
    public async Task<decimal> GetMaxPriceAsync(
        Guid? productId = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query =
            from price in dbContext.Set<ProductPrice>()
            select price;
        
        if (productId != null)
        {
            query = query.Where(x => x.ProductId == productId);
        }
        
        var maxPrice = await query.MaxAsync(x => x.Amount, GetCancellationToken(cancellationToken));
        return maxPrice;
    }
    
    public async Task<IQueryable<ProductPrice>> GetPriceQueryableAsync(
        Guid? productId = null)
    {
        var dbContext = await GetDbContextAsync();
        var query =
            from price in dbContext.Set<ProductPrice>()
            select price;

        if (productId != null)
        {
            query = query.Where(x => x.ProductId == productId);
        }

        return query;
    }

    public async Task<ProductPrice> GetPriceAsync(
        Guid productId,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        var query = await GetPriceQueryableAsync(productId);
        query = query.Where(x => x.Date == date);

        var price = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return price;
    }

    public async Task<List<ProductPrice>> GetPriceListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? productId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetPriceQueryableAsync(productId);

        if (string.IsNullOrWhiteSpace(sorting))
        {
            sorting = nameof(ProductPrice.Date) + " DESC";
        }

        query = query.OrderBy(sorting);
        query = query.PageBy(skipCount, maxResultCount);

        var list = await query.ToListAsync(GetCancellationToken(cancellationToken));
        return list;
    }

    public async Task<long> GetPriceCountAsync(
        Guid? productId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetPriceQueryableAsync(productId);

        var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }

    #endregion
}