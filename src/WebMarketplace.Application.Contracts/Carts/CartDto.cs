using System.Collections.Generic;

namespace WebMarketplace.Carts;

public class CartDto
{
    public decimal TotalPrice { get; set; }
    public List<CartItemDto> Items { get; set; }

    public CartDto()
    {
        Items = new List<CartItemDto>();
    }
}