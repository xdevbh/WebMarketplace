using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Organizations;

public class CreateUpdateOrganizationDto
{
    [Required] 
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;
    
    public string? ImagePath { get; set; } = string.Empty;
}   
