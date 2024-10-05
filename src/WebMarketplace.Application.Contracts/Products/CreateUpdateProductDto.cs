using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Products;

public class CreateUpdateProductDto
{
    [Required]
    public Guid CompanyId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public ProductCategory ProductCategory { get; set; }
    
    [Required]
    public ProductType ProductType { get; set; }
    
    public string? ShortDescription { get; set; }
    
    public string? FullDescription { get; set; }
    
}