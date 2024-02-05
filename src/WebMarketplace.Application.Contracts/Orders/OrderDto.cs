using System;
using Volo.Abp.Application.Dtos;
using WebMarketplace.Orders;

namespace WebMarketplace.Orders;

public class OrderDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; } 
}