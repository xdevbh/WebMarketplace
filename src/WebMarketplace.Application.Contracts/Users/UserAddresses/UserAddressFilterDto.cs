using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Users.UserAddresses;

public class UserAddressFilterDto : PagedAndSortedResultRequestDto
{
    public Guid? AddressId { get; set; }
    public Guid? UserId { get; set; }
}