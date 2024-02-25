using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Vendors;

public interface IUserVendorAppService : ICrudAppService
    <UserVendorDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserVendorDto>
{
}