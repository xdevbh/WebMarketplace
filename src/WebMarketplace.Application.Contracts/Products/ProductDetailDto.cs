using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductDetailDto: EntityDto<Guid>
{
    public Guid VendorId { get; set; }
    
    public string VendorName { get; set; }

    public string Name { get;  set; }

    public ProductCategory ProductCategory { get; set; }

    public ProductType ProductType { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }
    
    public double Rating { get; set; }

    public decimal PriceAmount { get; set; }
    
    public string PriceCurrency { get; set; }
    
    // public  PagedResultDto<ProductReviewDto> ProductReviews { get; set; }

}