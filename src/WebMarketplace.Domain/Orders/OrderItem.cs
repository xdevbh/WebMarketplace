using System;
using System.ComponentModel;
using Scriban.Parsing;
using Volo.Abp.Domain.Entities.Auditing;
using WebMarketplace.Products;

namespace WebMarketplace.Orders;

public class OrderItem: AuditedAggregateRoot<Guid>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
    
    public Product Product { get; set; }
    public Order Order { get; set; }
}