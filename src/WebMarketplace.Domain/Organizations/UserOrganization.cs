using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace WebMarketplace.Organizations;

public class UserOrganization : AuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }
}