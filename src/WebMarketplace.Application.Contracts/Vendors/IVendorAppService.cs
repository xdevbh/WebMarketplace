using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebMarketplace.Common;

namespace WebMarketplace.Vendors;

public interface IVendorAppService : ICrudAppService
    <VendorDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateVendorDto>
{
    Task<ListResultDto<VendorDto>> GetAllVendorsAsync();
    Task<PagedResultDto<VendorDto>> GetFilteredListAsync(VendorRequestDto vendorRequestDto);
    Task<ListResultDto<SelectOptionDto>> GetSelectOptionListAsync();
}