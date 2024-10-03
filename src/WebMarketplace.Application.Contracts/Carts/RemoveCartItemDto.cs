using System;

namespace WebMarketplace.Carts;

public class RemoveCartItemDto
{
    public Guid ProductId { get; set; }

    public int? Quantity { get; set; }
}