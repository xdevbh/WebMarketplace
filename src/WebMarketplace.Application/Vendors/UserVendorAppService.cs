using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Vendors;

public class UserVendorAppService : CrudAppService
    <UserVendor, UserVendorDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserVendorDto>
{
    public UserVendorAppService(IRepository<UserVendor, Guid> repository)
        : base(repository)
    {
    }
}