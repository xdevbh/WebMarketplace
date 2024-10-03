using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Orders;

public class OrderItemDto : EntityDto<Guid>
{
    public Guid OrderId { get; set; } // ?
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Currency { get; set; }
    public decimal TotalPrice { get; set; }
}