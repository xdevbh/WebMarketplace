using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies;

public class CompanyDto : EntityDto<Guid>
{
    public  string Name { get; set; }
    public  string DisplayName { get; set; }
    public  Guid AddressId { get; set; }
    public  string? Description { get; set; }
    public  string? Website { get; set; }
}