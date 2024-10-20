using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace WebMarketplace.Products;

public interface IProductBuyerAppService : IApplicationService
{
    // products
    Task<ProductDetailDto> GetProductDetailAsync(Guid id); // get Product Card
    Task<ProductCardListResultDto> GetProductCardListAsync(ProductCardListFilterDto input); // Get list of Products
    Task<PagedResultDto<ProductCardDto>> GetSimilarProductCardListAsync(SimilarProductCardListFilterDto input); // Get list of similar products
    
    // reviews
    Task CreateReview(CreateUpdateProductReviewBuyerDto input);
    Task<PagedResultDto<ProductReviewDto>> GetReviewListAsync(ProductReviewListFilterDto input);
    Task<bool> HasReviewAsync(Guid productId);
    
    // images
    Task<ProductImageDto> GetDefaultImageAsync(Guid productId);
    Task <ListResultDto<ProductImageDto>> GetAllImagesAsync(Guid productId);
}