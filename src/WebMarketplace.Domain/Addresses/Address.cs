using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Addresses;

public class Address : AuditedAggregateRoot<Guid>
{
    public virtual string FullName { get; private set; }
    public virtual string Country { get; private set; }
    public virtual string State { get; private set; }
    public virtual string City { get; private set; }
    public virtual string Line1 { get; private set; }
    public virtual string? Line2 { get; private set; }
    public virtual string ZipCode { get; private set; }
    public virtual string? Note { get; private set; }
    public virtual string PhoneNumber { get; private set; }
    public virtual string? Email { get; private set; }
    
    protected Address()
    {
    }

    public Address(
        Guid id, 
        string fullName, 
        string country, 
        string state, 
        string city, 
        string line1, 
        string? line2,
        string zipCode, 
        string? note, 
        string phoneNumber, 
        string? email)
        : base(id)
    {
        SetFullName(fullName);
        SetCountry(country);
        SetState(state);
        SetCity(city);
        SetLine1(line1);
        SetLine2(line2);
        SetZipCode(zipCode);
        SetNote(note);
        SetPhoneNumber(phoneNumber);
        SetEmail(email);
    }

    public Address SetFullName(string fullName)
    {
        FullName = Check.NotNullOrWhiteSpace(fullName, nameof(fullName));
        return this;
    }

    public Address SetCountry(string country)
    {
        Country = Check.NotNullOrWhiteSpace(country, nameof(country));
        return this;
    }

    public Address SetState(string state)
    {
        State = Check.NotNullOrWhiteSpace(state, nameof(state));
        return this;
    }

    public Address SetCity(string city)
    {
        City = Check.NotNullOrWhiteSpace(city, nameof(city));
        return this;
    }

    public Address SetLine1(string line1)
    {
        Line1 = Check.NotNullOrWhiteSpace(line1, nameof(line1));
        return this;
    }

    public Address SetLine2(string? line2)
    {
        Line2 = line2;
        return this;
    }

    public Address SetZipCode(string zipCode)
    {
        ZipCode = Check.NotNullOrWhiteSpace(zipCode, nameof(zipCode));
        return this;
    }

    public Address SetPhoneNumber(string phoneNumber)
    {
        PhoneNumber = Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
        return this;
    }

    public Address SetEmail(string? email)
    {
        Email = email;
        return this;
    }
    
    public Address SetNote(string? note)
    {
        Note = note;
        return this;
    }
}