using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Products;

public interface IProductBuyerAppService : IApplicationService
{
    // products
    Task<ProductDetailDto> GetProductDetailAsync(Guid id); // get Product Card
    Task<PagedResultDto<ProductCardDto>> GetProductCardListAsync(ProductCardListFilterDto input); // Get list of Products
    
    // reviews
    Task CreateReview(CreateUpdateProductReviewDto input);
    Task<PagedResultDto<ProductReviewDto>> GetReviewListAsync(ProductReviewListFilterDto input); 
    
    // images
    Task<ProductImageDto> GetDefaultImageAsync(Guid productId);
    Task <ListResultDto<ProductImageDto>> GetAllImagesAsync(Guid productId);
}