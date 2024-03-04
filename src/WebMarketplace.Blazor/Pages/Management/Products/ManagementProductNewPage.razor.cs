using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using WebMarketplace.Currencies;
using WebMarketplace.Permissions;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;

namespace WebMarketplace.Blazor.Pages.Management.Products;

public partial class ManagementProductNewPage
{
    private IFluentSpacingOnBreakpointWithSideAndSize commonMargin = Margin.Is3.FromBottom;
    private bool CanCreate { get; set; } = false;
    private Validations CreateValidationsRef { get; set; }

    private bool productInfoVisible { get; set; } = true;
    private bool productInventoryVisible { get; set; } = false;
    private bool productMediaVisible { get; set; } = false;
    private bool productPriceVisible { get; set; } = true;
    private bool productPublishingVisible { get; set; } = false;

    // [Parameter] public Guid? Id { get; set; }
    private CreateUpdateProductDto NewProduct { get; set; } = new CreateUpdateProductDto();
    private IEnumerable<ProductCategoryLookupDto> Categories { get; set; } = new List<ProductCategoryLookupDto>();
    private IEnumerable<CurrencyLookupDto> Currencies { get; set; } = new List<CurrencyLookupDto>();

    protected override async Task OnInitializedAsync()
    {
        await GetCategoriesAsync();
        await GetCurrenciesAsync();
        await SetPermissionsAsync();
        NewProduct = new CreateUpdateProductDto();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreate = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Products.Create);
    }

    protected async Task GetCategoriesAsync()
    {
        var result = await ProductCategoryAppService.GetLookupListAsync();
        Categories = result.Items;
    }

    protected async Task GetCurrenciesAsync()
    {
        var result = await CurrencyAppService.GetLookupListAsync();
        Currencies = result.Items;
    }

    protected async Task SaveProductAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await ProductAppService.CreateAsync(NewProduct);
            GoToProductList();
        }
    }
    
    private void GoToProductList()
    {
        NavigationManager.NavigateTo("/management/product/list");
    }
}