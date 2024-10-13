using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebMarketplace.Orders;
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
            .Include(x => x.Prices)
            .Include(x => x.Reviews)
            .Include(x => x.Images);
    }
    
    public static IQueryable<Order> IncludeDetails(this IQueryable<Order> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x=>x.Items);
    }
}