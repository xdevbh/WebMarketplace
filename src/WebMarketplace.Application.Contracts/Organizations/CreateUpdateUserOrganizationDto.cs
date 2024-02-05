using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Organizations;

public class CreateUpdateUserOrganizationDto
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid OrganizationId { get; set; }
}   
