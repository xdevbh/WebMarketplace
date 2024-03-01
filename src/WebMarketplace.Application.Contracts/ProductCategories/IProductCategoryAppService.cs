using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebMarketplace.Products;

namespace WebMarketplace.ProductCategories;

public interface IProductCategoryAppService : ICrudAppService
    <ProductCategoryDto, Guid>
{
    Task<List<ProductCategoryLookupDto>> GetLookupListAsync();

}