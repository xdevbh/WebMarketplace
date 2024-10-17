using System;

namespace WebMarketplace.Orders;

public class OrderCardDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }
}