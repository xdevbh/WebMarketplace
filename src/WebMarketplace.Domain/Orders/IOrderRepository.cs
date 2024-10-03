using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Orders
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<IQueryable<Order>> GetQueryableAsync(
            Guid? buyerId = null,
            Guid? addressId = null,
            OrderStatus? status = null
        );
    }
}
