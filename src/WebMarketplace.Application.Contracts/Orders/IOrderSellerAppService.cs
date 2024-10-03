using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Orders;

public interface IOrderSellerAppService: IApplicationService
{
    Task<OrderDto> GetAsync(Guid id);
    Task<List<OrderCardDto>> GetListAsync(OrderFilterDto input);
    Task<OrderDto> CreateAsync(CreateUpdateOrderDto input);
    Task<OrderDto> UpdateAsync(CreateUpdateOrderDto input);
    Task<OrderDto> ChangeStatusAsync(ChangeStatusDto input);
    Task<OrderDto> CancelAsync(Guid id);
    Task DeleteAsync(Guid id);
}