using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using WebMarketplace.Addresses;

namespace WebMarketplace.Companies;

public class Company : FullAuditedAggregateRoot<Guid>
{
    public virtual string CompanyIdentificationNumber { get; private set; }
    public virtual string Name { get; private set; }
    public virtual string DisplayName { get; private set; }
    public virtual Guid AddressId { get; set; }
    public virtual string? ShortDescription { get; set; }
    public virtual string? FullDescription { get; set; }
    public virtual string? Website { get; set; }

    // todo: add contact information as list with ContactInformationType: mail, phone ... 

    protected Company() { }

    public Company(
        Guid id, 
        string companyIdentificationNumber,
        string name, 
        string displayName, 
        Guid addressId, 
        string? shortDescription, 
        string? fullDescription, 
        string? website)
    :base(id)
    {
        SetIdentificationNumber(companyIdentificationNumber);
        SetName(name);
        SetDisplayName(displayName);
        AddressId = addressId;
        ShortDescription = shortDescription;
        FullDescription = fullDescription;
        Website = website;
    }
    
    public Company SetIdentificationNumber(string companyIdentificationNumber)
    {
        CompanyIdentificationNumber  = Check.NotNullOrWhiteSpace(companyIdentificationNumber, nameof(companyIdentificationNumber));
        return this;
    }

    public Company SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        return this;
    }
    
    public Company SetDisplayName(string displayName)
    {
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName));
        return this;
    }
    
    
    // public string? ImagePath { get; set; }
    // public VendorCategory VendorCategorygory { get; set; }
}