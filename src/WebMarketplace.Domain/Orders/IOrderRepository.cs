using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Orders
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<IQueryable<Order>> GetFilteredQueryableAsync(
            Guid? companyId = null,
            Guid? buyerId = null,
            OrderStatus? status = null);

        Task<List<Order>> GetFilteredListAsync(
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            Guid? companyId = null,
            Guid? buyerId = null,
            OrderStatus? status = null,
            CancellationToken cancellationToken = default);

        Task<long> GetFilteredCountAsync(
            Guid? companyId = null,
            Guid? buyerId = null,
            OrderStatus? status = null,
            CancellationToken cancellationToken = default);

        Task<List<Guid>> GetOrderedProductIdListAsync(
            Guid buyerId,
            CancellationToken cancellationToken = default);
    }
}
