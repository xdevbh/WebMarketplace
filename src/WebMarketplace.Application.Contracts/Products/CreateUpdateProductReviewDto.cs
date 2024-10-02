using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace WebMarketplace.Products;

public class CreateUpdateProductReviewDto
{
    [Required]
    public virtual Guid UserId { get; set; }
    
    [Required]
    public virtual Guid ProductId { get; set; }
    
    [Required]
    public virtual double Rating { get;  set; }
    
    public virtual string? Comment { get; set; }
}