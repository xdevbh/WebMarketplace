using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Orders;

public class OrderItemDto : FullAuditedEntityDto<Guid>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
}