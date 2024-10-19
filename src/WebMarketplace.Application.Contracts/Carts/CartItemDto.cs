using System;
using System.Collections.Generic;

namespace WebMarketplace.Carts;

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }

    public Guid CompanyId { get; set; }
    
    public string CompanyName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }
}