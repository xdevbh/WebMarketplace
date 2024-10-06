using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Products;

public interface IProductAdminAppService : IApplicationService
{
    // products
    Task DeleteProduct(Guid id);
    
    // reviews
    Task DeleteReview(DeleteProductReviewDto input); // todo 
    
    // images
    Task<ProductImageDto> GetDefaultImageAsync(Guid productId);
    Task <ListResultDto<ProductImageDto>> GetAllImagesAsync(Guid productId);
    Task CreateProductImageAsync(CreateProductImageDto input);
    Task DeleteProductImageAsync(DeleteProductImageDto input);
}