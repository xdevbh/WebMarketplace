using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Products;

public class ProductImage : CreationAuditedEntity
{
    public virtual Guid ProductId { get; set; }
    public virtual string BlobName { get; set; }
    public virtual bool IsDefault { get; set; }

    protected ProductImage()
    {
    }

    public ProductImage(Guid productId, string blobName, bool isDefault)
    {
        ProductId = productId;
        BlobName = blobName;
        IsDefault = isDefault;
    }

    public override object?[] GetKeys()
    {
        return new object?[] { ProductId, BlobName };
    }
}