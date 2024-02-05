using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Products;

public class CreateUpdateProductDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;
    
    public string? ImagePath { get; set; } = string.Empty;
    
    public Guid ProductCategoryId { get; set; }
    
    [Required]
    public Guid CompanyId { get; set; }
}