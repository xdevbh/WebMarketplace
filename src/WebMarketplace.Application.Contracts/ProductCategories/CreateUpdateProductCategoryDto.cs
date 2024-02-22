using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.ProductCategories;

public class CreateUpdateProductCategoryDto
{
    [Required]
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Description { get; set; }
}