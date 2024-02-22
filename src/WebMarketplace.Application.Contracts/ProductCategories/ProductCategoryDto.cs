using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.ProductCategories;

public class ProductCategoryDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Description { get; set; }
}