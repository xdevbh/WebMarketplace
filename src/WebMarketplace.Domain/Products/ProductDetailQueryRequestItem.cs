using System;
using Volo.Abp.Auditing;

namespace WebMarketplace.Products;

public class ProductDetailQueryRequestItem : IHasCreationTime
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string Name { get; set; }

    public ProductCategory ProductCategory { get; set; }

    public ProductType ProductType { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public bool IsPublished { get; set; }

    public double Rating { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; }
    public DateTime? PriceDate { get; set; }
    
    public string DefaultImageBlobName { get; set; }
    public DateTime CreationTime { get; set; }
}