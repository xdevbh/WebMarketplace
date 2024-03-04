using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WebMarketplace.Currencies;
using WebMarketplace.Permissions;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;
using WebMarketplace.Vendors;

namespace WebMarketplace.Blazor.Pages.Management.Products;

public partial class ManagementProductEditPage
{
    
    [Parameter] public string Id { get; set; }
    private VendorDto Vendor { get; set; } = new VendorDto();

    public Guid? ProductId { get; set; }
    private IFluentSpacingOnBreakpointWithSideAndSize commonMargin = Margin.Is3.FromBottom;

    private bool productInfoVisible { get; set; } = true;
    private bool productInventoryVisible { get; set; } = false;
    private bool productMediaVisible { get; set; } = false;
    private bool productPriceVisible { get; set; } = true;
    private bool productPublishingVisible { get; set; } = false;
    private CreateUpdateProductDto NewProduct { get; set; } = new CreateUpdateProductDto();
    private IEnumerable<ProductCategoryLookupDto> Categories { get; set; } = new List<ProductCategoryLookupDto>();
    private IEnumerable<CurrencyLookupDto> Currencies { get; set; } = new List<CurrencyLookupDto>();    
    private bool CanEdit { get; set; } = false;
    private Validations EditValidationsRef { get; set; }


    protected override async Task OnInitializedAsync()
    {
        if (Guid.TryParse(Id, out Guid productId))
        {
            ProductId = productId;
        }

        await GetVendorAsync();
        await SetPermissionsAsync();
        await GetProductAsync();
        await GetCategoriesAsync();
        await GetCurrenciesAsync();
    }
    
    private async Task GetVendorAsync()
    {
        var user = CurrentUser;
        if (user.Id.HasValue)
            Vendor = await UserVendorAppService.GetVendorByUserAsync(user.Id.Value);
    }

    private async Task SetPermissionsAsync()
    {
        CanEdit = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Products.Edit);
    }

    protected async Task GetProductAsync()
    {
        if (ProductId.HasValue)
        {
            var product = await ProductAppService.GetAsync(ProductId.Value);
            if (product != null)
            {
                NewProduct = ObjectMapper.Map<ProductDto, CreateUpdateProductDto>(product);
            }
        }
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

    private async Task SaveProductAsync(MouseEventArgs arg)
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await ProductAppService.UpdateAsync(ProductId.Value, NewProduct);
            GoToProductList();
        }
    }

    private void GoToProductList()
    {
        NavigationManager.NavigateTo("/management/product/list");
    }
}