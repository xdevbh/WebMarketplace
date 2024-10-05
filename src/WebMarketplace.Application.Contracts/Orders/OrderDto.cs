using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Orders;

public class OrderDto : CreationAuditedEntityDto<Guid>
{
    public Guid BuyerId { get; set; }
    public Guid AddressId { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }
    
    public List<OrderItemDto> Items { get; set; }
}