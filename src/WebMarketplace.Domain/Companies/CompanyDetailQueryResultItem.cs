using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace WebMarketplace.Companies;

public class CompanyDetailQueryResultItem : IHasCreationTime
{
    public Guid Id { get; set; }
    public string CompanyIdentificationNumber { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Website { get; set; }
    public Guid AddressId { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Line1 { get; set; }
    public string? Line2 { get; set; }
    public string ZipCode { get; set; }
    
    public string DefaultImageBlobName { get; set; }

    public DateTime CreationTime { get; set; }
}