using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductCardDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public double Rating { get; set; }
    public string? ShortDescription { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; }

    public byte[]? ImageContent { get; set; }
    
    public string ImageContentType { get; } = "application/octet-stream";
}