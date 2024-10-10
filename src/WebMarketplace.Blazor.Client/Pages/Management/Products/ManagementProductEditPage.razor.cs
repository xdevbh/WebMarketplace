using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using WebMarketplace.Blazor.Client.Pages.Administration.Companies.CompanyMemberships;
using WebMarketplace.Permissions;
using WebMarketplace.Products;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace WebMarketplace.Blazor.Client.Pages.Management.Products
{
    public partial class ManagementProductEditPage
    {

        [Parameter] public Guid Id { get; set; }

        protected bool CanEdit { get; set; } = false;

        protected override Task OnInitializedAsync()
        {
            PageTitle = L["Page:UpdateProduct"];
            PageHeader = L["Header:UpdateProduct"];
            return base.OnInitializedAsync();
        }

        protected override Task SetToolBarAsync()
        {
            PageToolbar.AddButton(L["Action:Cancel"],
                () => Cancel(),
                IconName.Times,
                Color.Danger);

            PageToolbar.AddButton(L["Action:Save"],
                () => UpdateAsync(),
                IconName.Save,
                Color.Primary,
                !CanEdit);

            return Task.CompletedTask;
        }

        protected override Task SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:Management"], "/management"));
            BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:Products"], "/management/product/list"));
            BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:UpdateProduct"]));

            return Task.CompletedTask;
        }

        protected override async Task SetPermissionsAsync()
        {
            CanEdit = await AuthorizationService.IsGrantedAsync(WebMarketplacePermissions.Products.Update);
            await base.SetPermissionsAsync();
        }

        protected async Task GetDataAsync()
        {
            await GetProductAsync();
            await GetPricesAsync();
            await Task.CompletedTask;
        }

        protected async Task UpdateAsync()
        {
            if (await ProductValidationsRef.ValidateAll())
            {
                var productDto = await ProductAppService.UpdateAsync(Id, EditProduct);

                await UiMessageService.Success(L["Message:SavedSuccessfully"]);
                await GoToList();
            }
        }

        protected Task Cancel()
        {
            GoToList();
            return Task.CompletedTask;
        }

        protected Task GoToList()
        {
            NavigationManager.NavigateTo("/management/product/list");
            return Task.CompletedTask;
        }

        #region Product

        protected ProductDto Product { get; set; } = new();
        protected CreateUpdateProductDto EditProduct { get; set; } = new();
        protected Validations ProductValidationsRef { get; set; } = new();

        protected async Task GetProductAsync()
        {
            Product = await ProductAppService.GetAsync(Id);
            EditProduct = ObjectMapper.Map<ProductDto, CreateUpdateProductDto>(Product);
        }

        #endregion

        #region Prices

        protected IReadOnlyList<ProductPriceDto> Prices { get; set; } = new List<ProductPriceDto>();
        protected CreateUpdateProductPriceDto EditPrice { get; set; } = new();
        protected ManagementProductPriceNewModal CreatePriceModalRef { get; set; }

        protected int PricePageSize { get; set; } = LimitedResultRequestDto.DefaultMaxResultCount;
        protected int PriceCurrentPage { get; set; }
        protected string PriceCurrentSorting { get; set; } = string.Empty;
        protected int PriceTotalCount { get; set; }

        protected async Task GetPricesAsync()
        {
            var priceFilter = new ProductPriceListFilterDto
            {
                MaxResultCount = PricePageSize,
                SkipCount = PriceCurrentPage * PricePageSize,
                Sorting = PriceCurrentSorting,
                ProductId = Id
            };
            var priceResult = await ProductAppService.GetPricesAsync(priceFilter);
            Prices = priceResult.Items;
            PriceTotalCount = (int)priceResult.TotalCount;
        }

        protected async Task OnPricesDataGridReadAsync(DataGridReadDataEventArgs<ProductPriceDto> e)
        {
            PricePageSize = e.PageSize;
            PriceCurrentPage = e.Page - 1;
            PriceCurrentSorting = string.Join(",", e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => $"{c.Field} {(c.SortDirection == SortDirection.Descending ? "DESC" : "")}"));

            await GetDataAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected async Task CreatePriceAsync()
        {
            await CreatePriceModalRef.Show();
        }

        protected async Task DeletePriceAsync(DateTime date)
        {
            await Task.CompletedTask;
        }

        #endregion
    }
}