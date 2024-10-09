using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Products;

public class CreateUpdateProductPriceDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(3)]
    public string Currency { get; set; }
}
