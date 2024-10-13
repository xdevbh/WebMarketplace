using System;

namespace WebMarketplace.Products;

public class DeleteProductImageDto
{
    public Guid ProductId { get; set; }

    public string BlobName { get; set; }
}