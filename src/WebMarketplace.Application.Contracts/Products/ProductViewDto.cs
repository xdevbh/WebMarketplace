using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using WebMarketplace.ProductCategories;
using WebMarketplace.Reviews;
using WebMarketplace.Vendors;

namespace WebMarketplace.Products;

public class ProductViewDto : AuditedEntityDto<Guid>
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
    public List<ReviewDto> Reviews { get; set; }
    public double AverageRating { get; set; }
    public int SoldCount { get; set; }

    public ProductCategoryDto ProductCategory { get; set; }
    public VendorDto Vendor { get; set; }
    
}