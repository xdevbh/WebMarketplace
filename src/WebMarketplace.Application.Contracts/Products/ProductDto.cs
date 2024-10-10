using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace WebMarketplace.Products;

public class ProductDto : AuditedEntityDto<Guid>
{
    public Guid CompanyId { get; set; }
    //public String CompanyName { get; set; }

    public string Name { get;  set; }

    public ProductCategory ProductCategory { get; set; }

    public ProductType ProductType { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public bool IsPublished { get;  set; }

}