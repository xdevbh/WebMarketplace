using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Companies;

public class CreateUpdateCompanyAdminDto
{
    [Required]
    public string CompanyIdentificationNumber { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string DisplayName { get; set; }
    
    [Required]
    public Guid AddressId { get; set; }
    
    public string? ShortDescription { get; set; }

    public  string? FullDescription { get; set; }    // to rich text editor

    public string? Website { get; set; }
}