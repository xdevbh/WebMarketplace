using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Orders;

public class OrderItemAppService : CrudAppService
    <OrderItem, OrderItemDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOrderItemDto>
{
    public OrderItemAppService(IRepository<OrderItem, Guid> repository)
        : base(repository)
    {
    }
}