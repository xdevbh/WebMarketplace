using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebMarketplace.Common;
using WebMarketplace.Products;

namespace WebMarketplace.ProductCategories;

public interface IProductCategoryAppService : ICrudAppService
    <ProductCategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductCategoryDto>
{
    Task<ListResultDto<ProductCategoryLookupDto>> GetLookupListAsync();
    Task<ListResultDto<ProductCategoryDto>> GetAllCategoriesAsync();
    Task<ListResultDto<SelectOptionDto>> GetSelectOptionListAsync();

}