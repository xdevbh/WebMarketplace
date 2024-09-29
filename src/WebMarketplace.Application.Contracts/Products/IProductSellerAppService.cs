using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Products;

public interface IProductSellerAppService : IApplicationService
{
    Task<ProductDto> GetAsync(Guid id);

    Task<List<ProductDto>> GetListAsync();

    Task<ProductDto> CreateAsync(CreateUpdateProductDto input);

    Task UpdateAsync(Guid id, CreateUpdateProductDto input);

    Task DeleteAsync(Guid id);
}