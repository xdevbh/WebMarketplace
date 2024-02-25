using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Reviews;

public class Review : AuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public double Rating { get; set; }
    public string? Content { get; set; }
}