using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Orders;

public class ShippingAddressDto
{
    [Required]
    public string FullName { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    [Required]
    public string State { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string Line1 { get; set; }
    public string? Line2 { get; set; }
    
    [Required]
    public string ZipCode { get; set; }
    public string? Note { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
}