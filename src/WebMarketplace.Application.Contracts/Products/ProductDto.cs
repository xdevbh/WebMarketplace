using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductDto : AuditedEntityDto<Guid>
{
    public Guid CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string Name { get;  set; }

    public ProductCategory ProductCategory { get; set; }

    public ProductType ProductType { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public double Rating { get; set; }

    public bool IsPublished { get; private set; }
    
    // public decimal PriceAmount { get; set; }
    
    // public string PriceCurrency { get; set; }

}