using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Companies.VendorUsers;

public class VendorUser : CreationAuditedAggregateRoot<Guid>
{
    public virtual Guid UserId { get; private set; }
    public virtual Guid CompanyId { get; private set; }

    protected VendorUser()
    {
    }

    public VendorUser(Guid id, Guid userId, Guid companyId)
        : base(id)
    {
        UserId = userId;
        CompanyId = companyId;
    }
}