using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.UserVendors;

public class CreateUpdateUserVendorDto
{
    [Required] public Guid UserId { get; set; }
    [Required] public Guid VendorId { get; set; }

    public Guid? Id { get; set; }
}