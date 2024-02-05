using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Reviews;

public class CreateUpdateReviewDto
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    [Required]
    public int Rating { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
}