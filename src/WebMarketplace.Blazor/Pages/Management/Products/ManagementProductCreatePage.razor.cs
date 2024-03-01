using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using WebMarketplace.Currencies;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;

namespace WebMarketplace.Blazor.Pages.Management.Products;

public partial class ManagementProductCreatePage
{
    [Parameter] public Guid? Id { get; set; }

    private bool productInfoVisible = true;
    private bool productPriceVisible = true;
    private bool productInventoryVisible = false;
    private bool productPublishingVisible = false;
    private bool productMediaVisible = false;
    private Validations CreateValidationsRef;
    private CreateUpdateProductDto Product { get; set; } = new CreateUpdateProductDto();
    private IEnumerable<ProductCategoryLookupDto> Categories { get; set; } = new List<ProductCategoryLookupDto>();
    private IEnumerable<CurrencyLookupDto> Currencies { get; set; } = new List<CurrencyLookupDto>();

    protected override async Task OnInitializedAsync()
    {
        await GetCategoriesAsync();
        await GetCurrenciesAsync();
    }

    protected async Task GetCategoriesAsync()
    {
        var tt = await ProductCategoryAppService.GetLookupListAsync();
        Categories = tt.AsEnumerable();
    }

    protected async Task GetCurrenciesAsync()
    {
        var result = await CurrencyAppService.GetLookupListAsync();
        Currencies = result.Items;
    }

    protected async Task CreateProductAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await ProductAppService.CreateAsync(Product);
        }
    }
}