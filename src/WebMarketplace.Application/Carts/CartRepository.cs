using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace WebMarketplace.Carts;

public class CartRepository : ICartRepository
{
    public bool? IsChangeTrackingEnabled { get; set; }

    private readonly IDistributedCache<Cart, Guid> _cache;

    public CartRepository(IDistributedCache<Cart, Guid> cache)
    {
        _cache = cache;
    }

    public async Task<Cart> GetAsync(Guid id)
    {
        return (await _cache.GetOrAddAsync(id, () => Task.FromResult(new Cart(id))))!;
    }

    public async Task UpdateAsync(Cart basket)
    {
        await _cache.SetAsync(basket.Id, basket);
    }
}