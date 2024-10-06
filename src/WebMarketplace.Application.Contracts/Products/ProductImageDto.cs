using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductImageDto : EntityDto
{
    public Guid ProductId { get; set; }
    public string BlobName { get; set; }
    public bool IsDefault { get; set; }
    public byte[] Content { get; set; }
}