using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies;

public class CompanyDto : EntityDto<Guid>
{
    public string CompanyIdentificationNumber { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public Guid AddressId { get; set; }
    public string? Short { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Website { get; set; }
}