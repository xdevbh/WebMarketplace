using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Vendors;

public interface IVendorAppService : ICrudAppService
    <VendorDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateVendorDto>
{
    Task<ListResultDto<VendorDto>> GetAllVendorsAsync();
    Task<PagedResultDto<VendorDto>> GetFilteredListAsync(VendorRequestDto vendorRequestDto);
}