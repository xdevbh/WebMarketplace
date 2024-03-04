using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Products;

public class CreateUpdateProductDto
{
    [Required] public string Name { get; set; }

    [Required] public double Price { get; set; }

    [Required] [StringLength(3)] 
    public string Currency { get; set; }

    public Guid VendorId { get; set; }
    public Guid? ProductCategoryId { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public double TaxPercent { get; set; }

    public int InStock { get; set; }
    public bool IsPublished { get; set; }
}