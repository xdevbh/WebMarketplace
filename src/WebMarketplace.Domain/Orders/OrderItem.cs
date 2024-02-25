using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Orders;

public class OrderItem : FullAuditedAggregateRoot<Guid>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
}