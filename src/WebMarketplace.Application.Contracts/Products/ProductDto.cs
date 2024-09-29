using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductDto : AuditedEntityDto<Guid>
{
    public Guid VendorId { get; set; }

    public string Name { get; set; }

    public ProductCategory ProductCategory { get; set; }

    public ProductType ProductType { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }
}