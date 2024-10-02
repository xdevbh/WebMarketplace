using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductReviewListFilterDto : PagedAndSortedResultRequestDto
{
    public Guid ProductId { get; set; }
    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }
}