using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Vendors;

public class VendorRequestDto : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }

}
