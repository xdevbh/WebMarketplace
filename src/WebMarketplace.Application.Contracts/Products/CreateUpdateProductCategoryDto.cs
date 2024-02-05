using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Products;

public class CreateUpdateProductCategoryDto
{
    [Required]
    public string Name { get; set; }
    
    public Guid? ParentCategoryId { get; set; }
}