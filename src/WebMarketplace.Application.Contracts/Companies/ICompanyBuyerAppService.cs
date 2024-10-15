using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Companies;

public interface ICompanyBuyerAppService : IApplicationService
{
    // Task<ListResultDto<CompanyDto>> GetAllAsync();
    Task<PagedResultDto<CompanyDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<ListResultDto<CompanySelectItemDto>> GetSelectItemListAsync();
    Task<CompanyDto> GetAsync(Guid id);
    Task<CompanyDto> GetByNameAsync(string name);
}