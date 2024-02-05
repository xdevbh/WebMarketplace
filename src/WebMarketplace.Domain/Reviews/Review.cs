using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using WebMarketplace.Products;

namespace WebMarketplace.Reviews;

public class Review : AuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public double Rating { get; set; }
    public string Content { get; set; }

    public Product Product { get; set; }
    public IdentityUser User { get; set; }
}