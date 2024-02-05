using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Organizations;

public class OrganizationAppService : CrudAppService
    <Organization, OrganizationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateOrganizationDto>
{
    public OrganizationAppService(IRepository<Organization, Guid> repository)
        : base(repository)
    {
    }
}