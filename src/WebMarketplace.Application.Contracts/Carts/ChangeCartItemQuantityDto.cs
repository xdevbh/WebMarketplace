using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Carts;

public class ChangeCartItemQuantityDto
{
    public Guid ProductId { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; } 
}