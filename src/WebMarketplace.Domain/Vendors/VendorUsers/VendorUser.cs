using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Vendors.VendorUsers;

public class VendorUser : CreationAuditedAggregateRoot<Guid>
{
    public Guid UserId { get; private set; }
    public Guid VendorId { get; private set; }

    protected VendorUser()
    {
    }

    public VendorUser(Guid id, Guid userId, Guid vendorId)
        : base(id)
    {
        UserId = userId;
        VendorId = vendorId;
    }
}