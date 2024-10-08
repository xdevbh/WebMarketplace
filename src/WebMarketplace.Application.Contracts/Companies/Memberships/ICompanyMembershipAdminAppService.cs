using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Companies.Memberships;

public interface ICompanyMembershipAdminAppService : IApplicationService
{
    Task<CompanyMembershipDto> GetAsync(Guid id);
    
    Task<PagedResultDto<CompanyMembershipDto>> GetListAsync(CompanyMembershipFilterDto input);
    
    Task<CompanyMembershipDto> CreateAsync(CreateCompanyMembershipDto input);
    
    Task DeleteAsync(Guid id);
}