using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Products;

public class ProductCategory : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    //todo: add description
}