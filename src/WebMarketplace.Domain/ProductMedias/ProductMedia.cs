using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.ProductMedias;

public class ProductMedia : AuditedAggregateRoot<Guid>
{
    public Guid ProductId { get; set; }
    public Guid BlobId { get; set; }
    public string Name { get; set; }
    public string Alt { get; set; }
}