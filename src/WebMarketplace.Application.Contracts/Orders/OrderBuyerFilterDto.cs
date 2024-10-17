using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Orders;

public class OrderBuyerFilterDto : PagedAndSortedResultRequestDto
{
    public OrderStatus? Status { get; set; }
}