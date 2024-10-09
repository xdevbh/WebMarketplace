using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Products;

public interface IProductSellerAppService : IApplicationService
{
    // products
    Task<ProductDto> GetAsync(Guid id);

    Task<PagedResultDto<ProductListItemDto>> GetListAsync(ProductListFilterDto input);

    Task<ProductDto> CreateAsync(CreateUpdateProductDto input);

    Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input);

    Task DeleteAsync(Guid id);

    Task PublishAsync(Guid id);

    Task UnpublishAsync(Guid id);

    // prices
    Task CreateProductPriceAsync(CreateUpdateProductPriceDto input);

    // images
    Task<ProductImageDto> GetDefaultImageAsync(Guid productId);
    Task <ListResultDto<ProductImageDto>> GetAllImagesAsync(Guid productId);
    Task CreateProductImageAsync(CreateProductImageDto input);
    Task DeleteProductImageAsync(DeleteProductImageDto input);
}