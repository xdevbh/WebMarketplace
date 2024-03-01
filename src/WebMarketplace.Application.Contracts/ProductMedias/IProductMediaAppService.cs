using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.ProductCategories;

public interface IProductMediaAppService : IApplicationService
{
    Task<ProductMediaDto> GetAsync(Guid id);

    Task<ListResultDto<ProductMediaDto>> GetListAsync(PagedAndSortedResultRequestDto input);

    Task<ProductMediaDto> CreateAsync(CreateProductMediaDto input);

    Task UpdateAsync(Guid id, UpdateProductMediaDto input);

    Task DeleteAsync(Guid id);
}