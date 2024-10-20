using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace WebMarketplace.Orders;

public class ShippingAddress : ValueObject
{
    public string FullName { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Line1 { get; set; }
    public string? Line2 { get; set; }
    public string ZipCode { get; set; }
    public string? Note { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }

    protected ShippingAddress()
    {
    }

    public ShippingAddress(
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

    #region Setters

    public ShippingAddress SetFullName(string fullName)
    {
        FullName = Check.NotNullOrWhiteSpace(fullName, nameof(fullName));
        return this;
    }

    public ShippingAddress SetCountry(string country)
    {
        Country = Check.NotNullOrWhiteSpace(country, nameof(country));
        return this;
    }

    public ShippingAddress SetState(string state)
    {
        State = Check.NotNullOrWhiteSpace(state, nameof(state));
        return this;
    }

    public ShippingAddress SetCity(string city)
    {
        City = Check.NotNullOrWhiteSpace(city, nameof(city));
        return this;
    }

    public ShippingAddress SetLine1(string line1)
    {
        Line1 = Check.NotNullOrWhiteSpace(line1, nameof(line1));
        return this;
    }

    public ShippingAddress SetLine2(string? line2)
    {
        Line2 = line2;
        return this;
    }

    public ShippingAddress SetZipCode(string zipCode)
    {
        ZipCode = Check.NotNullOrWhiteSpace(zipCode, nameof(zipCode));
        return this;
    }

    public ShippingAddress SetPhoneNumber(string phoneNumber)
    {
        PhoneNumber = Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
        return this;
    }

    public ShippingAddress SetEmail(string? email)
    {
        Email = email;
        return this;
    }

    public ShippingAddress SetNote(string? note)
    {
        Note = note;
        return this;
    }

    #endregion

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return FullName;
        yield return Country;
        yield return State;
        yield return City;
        yield return Line1;
        yield return Line2;
        yield return ZipCode;
        yield return Note;
        yield return PhoneNumber;
        yield return Email;
    }
}