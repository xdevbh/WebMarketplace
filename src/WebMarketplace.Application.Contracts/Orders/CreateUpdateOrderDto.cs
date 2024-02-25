using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Orders;

public class CreateUpdateOrderDto
{
    [Required] public Guid UserId { get; set; }

    [Required] public OrderStatus Status { get; set; }
}