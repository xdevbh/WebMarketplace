using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Orders;

public class OrderDto : FullAuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
}