using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Organizations;

public interface IOrganizationAppService : ICrudAppService
    <OrganizationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOrganizationDto>
{
}
