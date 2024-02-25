using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Vendors;

public class Vendor : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }

    public string Address { get; set; }
    // public Guid? VendorCategoryId { get; set; }
}