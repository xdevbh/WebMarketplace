using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Carts;

public class AddCartItemDto
{
    public Guid ProductId { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

}