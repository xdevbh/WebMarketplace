using System;

namespace WebMarketplace.Orders;

public class ChangeStatusDto
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}