using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using WebMarketplace.Vendors;

namespace WebMarketplace.UserVendors;

public class UserVendorDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid VendorId { get; set; }

    public VendorDto Vendor { get; set; }
    public IdentityUserDto User { get; set; }
}