using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Orders;

public interface IOrderBuyerAppService : IApplicationService
{
    Task<OrderDto> GetAsync(Guid id);
    Task<PagedResultDto<OrderCardDto>> GetListAsync(OrderBuyerFilterDto input);
    Task<OrderDto> CreateAsync(CreateOrderBuyerDto input);
    Task<OrderDto> CancelAsync(Guid id);
    Task<bool> HasPurchasedProductAsync (Guid id);
}