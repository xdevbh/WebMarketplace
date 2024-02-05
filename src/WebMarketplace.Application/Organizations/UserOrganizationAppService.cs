using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Organizations;

public class UserOrganizationAppService : CrudAppService
    <UserOrganization, UserOrganizationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserOrganizationDto>
{
    public UserOrganizationAppService(IRepository<UserOrganization, Guid> repository)
        : base(repository)
    {
    }
}