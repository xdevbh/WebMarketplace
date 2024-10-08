using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace WebMarketplace.Companies.Memberships;

public class CompanyMembershipDetailQueryResultItem : IHasCreationTime
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyDisplayName { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime CreationTime { get; set; }
}