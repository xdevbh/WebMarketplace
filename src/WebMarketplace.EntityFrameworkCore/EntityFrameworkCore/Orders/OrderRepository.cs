using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebMarketplace.Orders;
using WebMarketplace.Products;

namespace WebMarketplace.EntityFrameworkCore.Orders;

public class OrderRepository : EfCoreRepository<WebMarketplaceDbContext, Order, Guid>, IOrderRepository
{
    public OrderRepository(IDbContextProvider<WebMarketplaceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Order>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    public async Task<IQueryable<Order>> GetQueryableAsync(
        Guid? buyerId = null,
        Guid? addressId = null,
        OrderStatus? status = null)
    {
        var query = await GetQueryableAsync();
        query = query
            .WhereIf(buyerId.HasValue, x => x.BuyerId == buyerId)
            .WhereIf(addressId.HasValue, x => x.AddressId == addressId)
            .WhereIf(status.HasValue, x => x.Status == status);

        return query;
    }

    public async Task<List<Order>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? buyerId = null,
        Guid? addressId = null,
        OrderStatus? status = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = await GetQueryableAsync(buyerId, addressId, status);

        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(Order.CreationTime) + " DESC";
        }
        
        query = query.OrderBy(sorting);
        query = query.PageBy(skipCount, maxResultCount);
        var list = await query.ToListAsync(GetCancellationToken(cancellationToken));
        return list;
    }
    
    
    public async Task<long> GetCountAsync(
        Guid? buyerId = null,
        Guid? addressId = null,
        OrderStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(buyerId, addressId, status);
        var count= await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }
    
    public async Task<IQueryable<OrderItemWithProduct>> GetOrderItemWithProductQueryableAsync(
        Guid? orderId,
        Guid? productId)
    {
        var query = 
            from items in DbContext.Set<OrderItem>()
            join products in DbContext.Set<Product>() on items.ProductId equals products.Id
            select new OrderItemWithProduct(items, products);
        
        query = query.WhereIf(orderId.HasValue, x => x.OrderItem.OrderId == orderId);
        query = query.WhereIf(productId.HasValue, x => x.OrderItem.ProductId == productId);

        return query;
    }
}