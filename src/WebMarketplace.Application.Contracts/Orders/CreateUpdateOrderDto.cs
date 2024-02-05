using System;
using System.ComponentModel.DataAnnotations;
using WebMarketplace.Orders;

namespace WebMarketplace.Orders;

public class CreateUpdateOrderDto
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public OrderStatus Status { get; set; } 
}