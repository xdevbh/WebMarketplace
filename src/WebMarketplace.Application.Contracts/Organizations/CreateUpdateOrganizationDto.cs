using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Organizations;

public class CreateUpdateOrganizationDto
{
    [Required] 
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public string Address { get; set; }
}   
