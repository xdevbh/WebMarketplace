using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Orders;

public class Order : FullAuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
}