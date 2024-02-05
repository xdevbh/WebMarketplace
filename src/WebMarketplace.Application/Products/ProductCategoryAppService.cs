using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Products;

public class ProductCategoryAppService : CrudAppService
    <ProductCategory, ProductCategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductCategoryDto>
{
    public ProductCategoryAppService(IRepository<ProductCategory, Guid> repository)
        : base(repository)
    {
    }
}