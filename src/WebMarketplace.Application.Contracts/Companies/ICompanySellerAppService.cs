using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Companies;

public interface ICompanySellerAppService : IApplicationService
{
    // Task<ListResultDto<CompanyDto>> GetAllAsync();
    // Task<PagedResultDto<CompanyDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    // Task<CompanyDto> GetByNameAsync(string name);

    Task<CompanyDto> GetAsync();
    Task<CompanyLookupDto> GetCompanyLookupAsync();
    Task<CompanyDto> UpdateAsync(UpdateCompanySellerDto input);
}