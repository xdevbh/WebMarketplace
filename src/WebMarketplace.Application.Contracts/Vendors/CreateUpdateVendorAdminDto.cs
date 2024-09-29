using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Vendors;

public class CreateUpdateVendorAdminDto
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string DisplayName { get; set; }
    
    [Required]
    public Guid AddressId { get; set; }
    
    public string? Description { get; set; }
    
    public string? Website { get; set; }
}