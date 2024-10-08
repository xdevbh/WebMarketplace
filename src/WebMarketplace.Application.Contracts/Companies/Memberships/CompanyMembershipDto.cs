using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies.Memberships;

public class CompanyMembershipDto : EntityDto<Guid>
{
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyDisplayName { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}