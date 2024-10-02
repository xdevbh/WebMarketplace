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

    Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input);

    Task DeleteAsync(Guid id);
    
    Task PublishAsync(Guid id);
    
    Task UnpublishAsync(Guid id);
    
     Task CreateProductPriceAsync(CreateUpdateProductPriceDto input);
}