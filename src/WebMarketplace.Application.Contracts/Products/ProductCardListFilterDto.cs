using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductCardListFilterDto : PagedResultRequestDto
{
    public Guid? CompanyId { get; set; }
    public string? Name { get; private set; }
    public ProductCategory? ProductCategory { get; set; }
    public ProductType? ProductType { get; set; }
    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }
    public decimal? MinPriceAmount { get; set; }
    public decimal? MaxPriceAmount { get; set; }
    public string? PriceCurrency { get; set; }
    public ProductSorting Sorting { get; set; }
}