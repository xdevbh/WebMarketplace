using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Products;

public interface IProductAppService : IApplicationService
{


    #region ForVendors
    
    Task<PagedResultDto<ProductCardDto>> GetVendorProductCardListAsync(PagedAndSortedResultRequestDto input);
    
    Task<ProductDto> GetAsync(Guid id);
    Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<PagedResultDto<ProductDto>> GetFilteredListAsync(ProductRequestDto input);
    Task<ProductDto> CreateAsync(CreateUpdateProductDto input);
    Task UpdateAsync(Guid id, CreateUpdateProductDto input);

    Task DeleteAsync(Guid id);

    #endregion

    #region ForCustomers

    Task<PagedResultDto<ProductCardDto>> GetFilteredProductCardListAsync(ProductRequestDto input);
    Task<ProductViewDto> GetProductViewAsync(Guid id);

    #endregion
}