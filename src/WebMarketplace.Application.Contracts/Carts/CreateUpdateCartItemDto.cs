using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Carts;

public class CreateUpdateCartItemDto
{
    [Required]
    public Guid CartId { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    [StringLength(3)]
    public string Currency { get; set; }
}