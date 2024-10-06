using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Products;

public class CreateProductImageDto
{
    [Required]
    public Guid ProductId { get; set; }

    public byte[] Content { get; set; }

    public bool IsDefault { get; set; } = false;
}