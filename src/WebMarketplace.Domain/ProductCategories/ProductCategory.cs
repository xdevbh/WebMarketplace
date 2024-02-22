using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.ProductCategories;

public class ProductCategory : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Description { get; set; }
}