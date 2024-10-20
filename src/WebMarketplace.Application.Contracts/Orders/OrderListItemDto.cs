using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;

namespace WebMarketplace.Orders;

public class OrderListItemDto : CreationAuditedEntityDto<Guid>
{
    public BuyerDto Buyer { get; set; } = new();
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }
}