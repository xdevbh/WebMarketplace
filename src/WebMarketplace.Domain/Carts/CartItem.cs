using System;
using Volo.Abp.Domain.Entities.Auditing;
using WebMarketplace.Products;

namespace WebMarketplace.Carts;

public class CartItem : AuditedAggregateRoot<Guid>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }

    public Cart Cart { get; set; }
    public Product Product { get; set; }
}