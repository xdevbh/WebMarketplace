using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebMarketplace.Products;

namespace WebMarketplace.EntityFrameworkCore;

public static class WebMarketplaceEfCoreQueryableExtensions
{
    public static IQueryable<Product> IncludeDetails(this IQueryable<Product> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x=>x.ProductPrices)
            .Include(x=>x.ProductReviews);
    }
}