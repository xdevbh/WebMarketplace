using System;

namespace WebMarketplace.Users.UserAddresses;

public class CreateUserAddressDto
{
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
}