using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using WebMarketplace.Currencies;
using WebMarketplace.Permissions;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;

namespace WebMarketplace.Blazor.Pages.Management.Products;

public partial class ManagementProductListPage
{
    private IFluentSpacingOnBreakpointWithSideAndSize commonMargin = Margin.Is3.FromBottom;
    private IReadOnlyList<ProductDto> Products { get; set; }
    private IReadOnlyList<ProductCategoryDto> Categories { get; set; } = new List<ProductCategoryDto>();
    private IReadOnlyList<CurrencyDto> Currencies { get; set; } = new List<CurrencyDto>();


    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }

    private bool CanCreateProduct { get; set; }
    private bool CanEditProduct { get; set; }
    private bool CanDeleteProduct { get; set; }

    #region filter
    private string? selectedNameFilter { get; set; }
    private double? selectedPriceFilter { get; set; }
    private Guid selectedCategoryFilter { get; set; }
    private int? selectedStockFilter { get; set; }
    private bool? selectedPublishedFilter { get; set; }
    private string? selectedCurrencyFilter { get; set; }
    #endregion

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetProductsAsync();
        await GetProductCategoriesAsync();
        await GetCurrenciesAsync();
    }

    private async Task GetProductCategoriesAsync()
    {
        var result = await ProductCategoryAppService.GetAllCategoriesAsync();
        Categories = result.Items;
    }
    
    private async Task GetCurrenciesAsync()
    {
        var result = await CurrencyAppService.GetListAsync();
        Currencies = result.Items;
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateProduct = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Products.Create);

        CanEditProduct = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Products.Edit);

        CanDeleteProduct = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Products.Delete);
    }

    private async Task GetProductsAsync()
    {
        var result = await ProductAppService.GetFilteredListAsync(new ProductRequestDto
        {
            MaxResultCount = PageSize,
            SkipCount = CurrentPage * PageSize,
            Sorting = CurrentSorting,
            Name = selectedNameFilter,
            Price = selectedPriceFilter,
            ProductCategoryId = selectedCategoryFilter == Guid.Empty ? null : selectedCategoryFilter,
            InStock  = selectedStockFilter,
            IsPublished = selectedPublishedFilter,
            Currency = selectedCurrencyFilter
        });

        Products = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ProductDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetProductsAsync();
        await InvokeAsync(StateHasChanged);
    }
    

    private void OpenCreateProduct()
    {
        NavigationManager.NavigateTo("/management/product/new");
    }

    private void OpenEditProduct(ProductDto context)
    {
        NavigationManager.NavigateTo($"/management/product/edit/{context.Id}");
    }

    private async Task DeleteProductDtoAsync(ProductDto context)
    {
        var confirmMessage = L["ProductDeletionConfirmationMessage"];
        var message = confirmMessage + " '" + context.Name + "'?";
        if (!await Message.Confirm(message))
        {
            return;
        }

        await ProductAppService.DeleteAsync(context.Id);
        await GetProductsAsync();
    }
}