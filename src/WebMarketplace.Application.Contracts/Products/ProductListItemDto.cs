using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductListItemDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public double Rating { get; set; }
    public string? ShortDescription { get; set; }
    public ProductCategory ProductCategory { get; set; }
    public ProductType ProductType { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; }
    public bool IsPublished { get;  set; }
}