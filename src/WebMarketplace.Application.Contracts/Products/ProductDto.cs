using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }

    public Guid ProductCategoryId { get; set; }
    public Guid CompanyId { get; set; }
}