using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Vendors;

public class CreateUpdateVendorDto
{
    [Required] public string Name { get; set; }
    [Required] public string Address { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}