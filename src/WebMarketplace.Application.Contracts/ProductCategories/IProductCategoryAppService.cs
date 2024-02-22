using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.ProductCategories;

public interface IProductCategoryAppService : ICrudAppService
    <ProductCategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductCategoryDto>
{
}