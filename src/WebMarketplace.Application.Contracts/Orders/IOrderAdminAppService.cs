using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Orders;

public interface IOrderAdminAppService : IApplicationService
{

    Task<OrderDto> GetAsync(Guid id);
    Task<List<OrderCardDto>> GetListAsync(OrderFilterSellerDto input);
    Task<OrderDto> ChangeStatusAsync(ChangeStatusDto input);
    Task DeleteAsync(Guid id);
}