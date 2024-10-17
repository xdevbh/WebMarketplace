using System;

namespace WebMarketplace.Orders;

public class CreateOrderItemBuyerDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Currency { get; set; }
}