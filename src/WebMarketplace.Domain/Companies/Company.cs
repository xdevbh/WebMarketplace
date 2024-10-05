using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using WebMarketplace.Addresses;

namespace WebMarketplace.Companies;

public class Company : FullAuditedAggregateRoot<Guid>
{
    public virtual string Name { get; private set; }
    public virtual string DisplayName { get; private set; }
    public virtual Guid AddressId { get; set; }
    
    public virtual string? Description { get; set; }
    public virtual string? Website { get; set; }

    // todo: add contact information as list with ContactInformationType: mail, phone ... 

    protected Company() { }

    public Company(
        Guid id, 
        string name, 
        string displayName, 
        Guid addressId, 
        string? description, 
        string? website)
    :base(id)
    {
        SetName(name);
        SetDisplayName(displayName);
        AddressId = addressId;
        Description = description;
        Website = website;
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