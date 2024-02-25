using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Vendors;

public class UserVendorDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid VendorId { get; set; }
}