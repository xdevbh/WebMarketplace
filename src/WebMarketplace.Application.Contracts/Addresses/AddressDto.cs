using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Addresses;

public class AddressDto : EntityDto<Guid>
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
}