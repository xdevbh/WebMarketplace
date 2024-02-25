using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Vendors;

public class CreateUpdateUserVendorDto
{
    [Required] public Guid UserId { get; set; }

    [Required] public Guid VendorId { get; set; }
}