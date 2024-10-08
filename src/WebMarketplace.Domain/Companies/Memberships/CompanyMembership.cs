using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Companies.Memberships;

public class CompanyMembership : CreationAuditedAggregateRoot<Guid>
{
    protected CompanyMembership()
    {
    }

    public CompanyMembership(Guid id, Guid companyId, Guid userId)
        : base(id)
    {
        UserId = userId;
        CompanyId = companyId;
    }

    public virtual Guid UserId { get; private set; }
    public virtual Guid CompanyId { get; private set; }
}