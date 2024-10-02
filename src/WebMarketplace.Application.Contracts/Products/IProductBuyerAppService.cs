using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Products;

public interface IProductBuyerAppService : IApplicationService
{
    Task<ProductDetailDto> GetProductDetailAsync(Guid id); // get Product Card
    Task<PagedResultDto<ProductCardDto>> GetProductCardListAsync(ProductCardListFilterDto input); // Get list of Products
    Task CreateReview(CreateUpdateProductReviewDto input);
    Task<PagedResultDto<ProductReviewDto>> GetReviewListAsync(ProductReviewListFilterDto input); 
}