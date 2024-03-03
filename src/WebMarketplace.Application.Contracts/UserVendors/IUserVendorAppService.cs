using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using WebMarketplace.Vendors;

namespace WebMarketplace.UserVendors;

public interface IUserVendorAppService : ICrudAppService
    <UserVendorDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserVendorDto>
{
    Task<PagedResultDto<UserVendorDto>> GetFilteredListAsync(UserVendorRequestDto input);
    Task<List<IdentityUserDto>> GetAllUsersAsync();
    Task<List<VendorDto>> GetAllVendorsAsync();
}
