using System;
using System.Collections.Generic;

namespace WebMarketplace.Orders;

public class CreateUpdateOrderDto
{
    public Guid AddressId { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; }
    public OrderStatus Status { get; set; }
    public decimal? TotalPrice { get; set; }
    
    public List<CreateUpdateOrderDto> Items { get; set; }
}