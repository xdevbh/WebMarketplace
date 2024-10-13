using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Products;

public class DeleteProductPriceDto
{
    public Guid ProductId { get; set; }

    public DateTimeOffset Date { get; set; }
}