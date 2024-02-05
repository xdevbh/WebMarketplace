using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Orders;

public class OrderItemDto : AuditedEntityDto<Guid>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
}