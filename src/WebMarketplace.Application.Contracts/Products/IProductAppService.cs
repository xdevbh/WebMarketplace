using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Products;

public interface IProductAppService : IApplicationService
{
    Task<ProductDto> GetAsync(Guid id);

    Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input);

    Task<ProductDto> CreateAsync(CreateUpdateProductDto input);

    Task UpdateAsync(Guid id, CreateUpdateProductDto input);

    Task DeleteAsync(Guid id);
}