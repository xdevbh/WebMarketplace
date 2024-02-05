using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Organizations;

public class UserOrganizationDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }
}