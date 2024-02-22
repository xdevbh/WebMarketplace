using System;
using Volo.Abp.Domain.Entities.Auditing;
using WebMarketplace.Organizations;
using WebMarketplace.ProductCategories;

namespace WebMarketplace.Products;

public class Product : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public Guid? ProductCategoryId { get; set; }
    public Guid OrganizationId { get; set; }
}