using System;

namespace WebMarketplace.Companies.Memberships;

public class CreateCompanyMembershipDto
{
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
}