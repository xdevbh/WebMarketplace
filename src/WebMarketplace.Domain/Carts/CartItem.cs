using System;
using Volo.Abp.Domain.Entities;

namespace WebMarketplace.Carts;

public class CartItem : Entity<Guid>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
}