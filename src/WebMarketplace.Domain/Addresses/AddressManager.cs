using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectExtending;

namespace WebMarketplace.Addresses;

public class AddressManager : DomainService
{
    public async Task<Address> CreateAsync(
        string fullName,
        string country,
        string state,
        string city,
        string line1,
        string zipCode,
        string phoneNumber,
        string? line2 = null,
        string? note = null,
        string? email = null)
    {
        var address = new Address(
            GuidGenerator.Create(),
            fullName,
            country,
            state,
            city,
            line1,
            line2,
            zipCode,
            note,
            phoneNumber,
            email
        );

        return address;
    }

    public async Task<Address> EditAsync(
        Address address, 
        string fullName, 
        string country, 
        string state, 
        string city,
        string line1, 
        string line2,
        string zipCode, 
        string? note,
        string phoneNumber, 
        string? email = null)
    {
        address.SetFullName(fullName);
        address.SetCountry(country);
        address.SetState(state);
        address.SetCity(city);
        address.SetLine1(line1);
        address.SetZipCode(zipCode);
        address.SetPhoneNumber(phoneNumber);
        address.SetLine2(line2);
        address.SetNote(note);
        address.SetEmail(email);

        return address;
    }
}