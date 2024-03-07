using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Common;
using WebMarketplace.Permissions;

namespace WebMarketplace.ProductCategories;

public class ProductCategoryAppService : CrudAppService
    <ProductCategory, ProductCategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductCategoryDto>,
    IProductCategoryAppService
{
    public ProductCategoryAppService(IRepository<ProductCategory, Guid> repository)
        : base(repository)
    {
        GetPolicyName = WebMarketplacePermissions.ProductCategories.Default;
        GetListPolicyName = WebMarketplacePermissions.ProductCategories.Default;
        CreatePolicyName = WebMarketplacePermissions.ProductCategories.Create;
        UpdatePolicyName = WebMarketplacePermissions.ProductCategories.Edit;
        DeletePolicyName = WebMarketplacePermissions.ProductCategories.Delete;
    }

    [Authorize(WebMarketplacePermissions.ProductCategories.Default)]
    public async Task<ListResultDto<ProductCategoryLookupDto>> GetLookupListAsync()
    {
        var categories = await Repository.GetListAsync();
        var dtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryLookupDto>>(categories);
        return new ListResultDto<ProductCategoryLookupDto>(dtos);
    }

    [Authorize(WebMarketplacePermissions.ProductCategories.Default)]
    async Task<ListResultDto<ProductCategoryDto>> IProductCategoryAppService.GetAllCategoriesAsync()
    {
        var categories = await Repository.GetListAsync();
        var dtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(categories);
        return new ListResultDto<ProductCategoryDto>(dtos);
    }

    public async Task<ListResultDto<SelectOptionDto>> GetSelectOptionListAsync()
    {
        var categories = await Repository.ToListAsync();

        var result = categories
            .Select(x => new SelectOptionDto
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

        return new ListResultDto<SelectOptionDto>(result);
    }
}