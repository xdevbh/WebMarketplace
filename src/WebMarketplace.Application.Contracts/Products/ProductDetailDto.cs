using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace WebMarketplace.Products;

public class ProductDetailDto: EntityDto<Guid>, IHasCreationTime
{
    public Guid CompanyId { get; set; }
    
    public string CompanyName { get; set; }

    public string Name { get;  set; }

    public ProductCategory ProductCategory { get; set; }

    public ProductType ProductType { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }
    
    public double Rating { get; set; }

    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; }
    
    public DateTime CreationTime { get; set; }
}