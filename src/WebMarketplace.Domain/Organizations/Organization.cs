using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using WebMarketplace.Products;

namespace WebMarketplace.Organizations;

public class Organization : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public string Address { get; set; }

    public List<Product> Products { get; set; }
    // public Guid? OrganizationCategoryId { get; set; }
}