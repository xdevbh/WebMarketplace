using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Orders;

public interface IOrderItemAppService : ICrudAppService
    <OrderItemDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOrderItemDto>
{
}