using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Orders;

public class OrderFilterSellerDto : PagedAndSortedResultRequestDto
{
    public OrderStatus? Status { get; set; }
}