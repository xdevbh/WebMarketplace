using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Vendors;

public class VendorDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public string Address { get; set; }
}