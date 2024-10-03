using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Carts;

public interface ICartRepository: IRepository
{
    Task<Cart> GetAsync(Guid id);

    Task UpdateAsync(Cart basket);
}