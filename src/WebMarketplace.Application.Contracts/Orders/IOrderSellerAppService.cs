using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Orders;

public interface IOrderSellerAppService: IApplicationService
{
    Task<OrderDto> GetAsync(Guid id);
    Task<PagedResultDto<OrderListItemDto>> GetListAsync(OrderFilterSellerDto input);
    Task<OrderDto> ChangeStatusAsync(ChangeStatusDto input);
}