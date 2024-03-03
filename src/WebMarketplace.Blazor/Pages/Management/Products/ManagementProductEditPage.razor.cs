using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;

namespace WebMarketplace.Blazor.Pages.Management.Products;

public partial class ManagementProductEditPage
{
    [Parameter] public Guid? Id { get; set; }
    private bool productInfoVisible = true;
    private bool productPriceVisible = true;
    private bool productInventoryVisible = false;
    private bool productPublishingVisible = false;
    private CreateUpdateProductDto Product { get; set; } = new CreateUpdateProductDto();
    private IEnumerable<ProductCategoryLookupDto> Categories { get; set; } = new List<ProductCategoryLookupDto>();

    protected override async Task OnInitializedAsync()
    {
        await GetProductAsync();
        await GetCategoriesAsync();
    }

    protected async Task GetProductAsync()
    {
        if (Id.HasValue)
        {
            var product = await ProductAppService.GetAsync(Id.Value);
            if (product != null)
            {
                Product = ObjectMapper.Map<ProductDto, CreateUpdateProductDto>(product);
            }
        }
    }

    protected async Task GetCategoriesAsync()
    {
        var tt = await ProductCategoryAppService.GetLookupListAsync();
        Categories = tt.AsEnumerable();
    }
}