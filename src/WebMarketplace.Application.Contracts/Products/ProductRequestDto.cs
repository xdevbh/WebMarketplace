using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductRequestDto : PagedAndSortedResultRequestDto
{
    public Guid? ProductCategoryId { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public string? Currency { get; set; }
    public int? InStock { get; set; }
    public bool? IsPublished { get; set; }
}