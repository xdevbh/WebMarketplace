using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Products;

public class CreateUpdateProductDto
{
    [Required] public Guid VendorId { get; set; }

    [Required] public string Name { get; set; }

    [Required] public double Price { get; set; }

    [Required] [StringLength(3)] public string Currency { get; set; }

    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public Guid? ProductCategoryId { get; set; }
}