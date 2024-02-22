using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductDto : AuditedEntityDto<Guid>
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
    
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public Guid? ProductCategoryId { get; set; }
}