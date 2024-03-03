using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.UserVendors;

public class UserVendorRequestDto : PagedAndSortedResultRequestDto
{
    public Guid? UserId { get; set; }
    public Guid? VendorId { get; set; }
}