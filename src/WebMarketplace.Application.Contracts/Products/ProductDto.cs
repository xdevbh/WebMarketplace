using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductDto : AuditedEntityDto<Guid>
{
    public Guid VendorId { get; set; }
    public string Name { get; set; }
    public Guid? ProductCategoryId { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
    public double TaxPercent { get; set; }

    public int InStock { get; set; }
    public bool IsPublished { get; set; }
}