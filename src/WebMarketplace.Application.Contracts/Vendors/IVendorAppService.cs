using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Vendors;

public interface IVendorAppService : ICrudAppService
    <VendorDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateVendorDto>
{
}