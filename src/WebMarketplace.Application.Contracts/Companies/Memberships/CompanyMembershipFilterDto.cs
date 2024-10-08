using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies.Memberships;

public class CompanyMembershipFilterDto : PagedAndSortedResultRequestDto
{
    public Guid? CompanyId { get; set; }
    public Guid? UserId { get; set; }
}