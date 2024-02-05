using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Organizations;

public interface IUserOrganizationAppService : ICrudAppService
    <UserOrganizationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserOrganizationDto>
{
}
