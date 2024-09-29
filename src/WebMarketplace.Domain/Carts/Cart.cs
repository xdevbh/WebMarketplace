using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Carts;

public class Cart : AuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
}