using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebMarketplace.Products;

namespace WebMarketplace.Companies;

public interface ICompanyAdminAppService : IApplicationService
{
    // Task<ListResultDto<CompanyDto>> GetAllAsync();
    Task<PagedResultDto<CompanyDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<CompanyDto> GetAsync(Guid id);
    Task<CompanyDto> GetByNameAsync(string name);
    Task<CompanyDto> UpdateAsync(Guid id, CreateUpdateCompanyAdminDto input);
    Task<CompanyDto> CreateAsync(CreateUpdateCompanyAdminDto input);
    Task DeleteAsync(Guid id);
}