using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Vendors;

public class UserVendor : AuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public Guid VendorId { get; set; }
}