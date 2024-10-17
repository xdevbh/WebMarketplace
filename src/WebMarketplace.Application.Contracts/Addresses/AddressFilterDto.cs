using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Addresses;

public class AddressFilterDto : PagedAndSortedResultRequestDto
{
    public Guid? UserId { get; set; }
}