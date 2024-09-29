using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebMarketplace.Products;

namespace WebMarketplace.Vendors;

public interface IVendorAdminAppService : IApplicationService
{
    // Task<ListResultDto<VendorDto>> GetAllAsync();
    Task<PagedResultDto<VendorDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<VendorDto> GetAsync(Guid id);
    Task<VendorDto> GetByNameAsync(string name);
    Task<VendorDto> UpdateAsync(Guid id, CreateUpdateVendorAdminDto input);
    Task<VendorDto> CreateAsync(CreateUpdateVendorAdminDto input);
    Task DeleteAsync(Guid id);
}