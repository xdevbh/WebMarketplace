using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.ProductCategories;

public class ProductCategoryAppService : CrudAppService
    <ProductCategory, ProductCategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductCategoryDto>
{
    public ProductCategoryAppService(IRepository<ProductCategory, Guid> repository)
        : base(repository)
    {
    }

    public override Task<ProductCategoryDto> UpdateAsync(Guid id, CreateUpdateProductCategoryDto input)
    {
        return base.UpdateAsync(id, input);     
    }
}