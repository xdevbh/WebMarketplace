using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Vendors;

public class CreateUpdateVendorDto
{
    [Required] public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public string Address { get; set; }
}