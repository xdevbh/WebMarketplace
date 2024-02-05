using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductCategoryDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
}