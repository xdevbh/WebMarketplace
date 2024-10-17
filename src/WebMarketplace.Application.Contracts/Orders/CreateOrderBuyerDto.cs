using System;
using System.Collections.Generic;

namespace WebMarketplace.Orders;

public class CreateOrderBuyerDto
{
    public Guid AddressId { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; }
    // public decimal? TotalPrice { get; set; }
    public List<CreateOrderItemBuyerDto> Items { get; set; }
}