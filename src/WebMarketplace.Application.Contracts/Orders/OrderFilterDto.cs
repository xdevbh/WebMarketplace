using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Orders;

public class OrderFilterDto : PagedAndSortedResultRequestDto
{
    public Guid? BuyerId { get; set; }
    public OrderStatus? Status { get; set; }
}