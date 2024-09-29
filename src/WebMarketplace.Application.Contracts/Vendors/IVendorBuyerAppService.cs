using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Vendors;

public interface IVendorBuyerAppService : IApplicationService
{
    // Task<ListResultDto<VendorDto>> GetAllAsync();
    Task<PagedResultDto<VendorDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<VendorDto> GetAsync(Guid id);
    Task<VendorDto> GetByNameAsync(string name);
}