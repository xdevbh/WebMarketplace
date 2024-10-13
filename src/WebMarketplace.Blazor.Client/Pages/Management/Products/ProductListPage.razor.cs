using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using WebMarketplace.Companies;
using WebMarketplace.Permissions;
using WebMarketplace.Products;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace WebMarketplace.Blazor.Client.Pages.Management.Products
{
    public partial class ProductListPage
    {

        protected IReadOnlyList<ProductListItemDto> Products { get; set; } = new List<ProductListItemDto>();
        protected CompanyLookupDto Company { get; set; } = new();
        protected bool CanPublish { get; set; } = false;
        protected string? SelectedId { get; set; } = null;
        protected string? SelectedName { get; set; } = null;
        protected ProductCategory SelectedProductCategory { get; set; } = ProductCategory.Undefined;
        protected ProductType SelectedProductType { get; set; } = ProductType.Undefined;
        protected double? SelectedMinRating { get; set; } = null;
        protected double? SelectedMaxRating { get; set; } = null;
        protected decimal? SelectedMinPriceAmount { get; set; } = null;
        protected decimal? SelectedMaxPriceAmount { get; set; } = null;
        public string SelectedPriceCurrency { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            await GetCompanyAsync();
            PageTitle = L["Page:Products"];
            PageHeader = L["Header:Products"] + ": " + Company.Name;
            await base.OnInitializedAsync();
        }

        protected override async Task SetPermissionsAsync()
        {
            CanView = await AuthorizationService.IsGrantedAsync(WebMarketplacePermissions.Products.Default);
            CanCreate = await AuthorizationService.IsGrantedAsync(WebMarketplacePermissions.Products.Create);
            CanEdit = await AuthorizationService.IsGrantedAsync(WebMarketplacePermissions.Products.Create);
            CanDelete = await AuthorizationService.IsGrantedAsync(WebMarketplacePermissions.Products.Create);
            CanPublish = await AuthorizationService.IsGrantedAsync(WebMarketplacePermissions.Products.Publish);
        }

        protected override Task SetToolBarAsync()
        {
            PageToolbar.AddButton(L["Action:ReloadData"],
                () => GetDataAsync(),
                IconName.Redo,
                Color.Secondary);

            PageToolbar.AddButton(L["Action:New"],
                () => CreateAsync(),
                IconName.Add,
                Color.Primary,
                !CanCreate);

            return Task.CompletedTask;
        }

        protected override Task SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:Management"], "/management"));
            BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:Products"], "/management/product/list"));

            return Task.CompletedTask;
        }

        protected override async Task GetDataAsync()
        {
            Guid? id = null;
            if (Guid.TryParse(SelectedId, out var parsed))
            {
                id = parsed;
            }

            var filter = new ProductListFilterDto()
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting,
                Id = id,
                Name = SelectedName,
                ProductCategory = SelectedProductCategory == ProductCategory.Undefined ? null : SelectedProductCategory,
                ProductType = SelectedProductType == ProductType.Undefined ? null : SelectedProductType,
                MinRating = SelectedMinRating,
                MaxRating = SelectedMaxRating,
                MinPriceAmount = SelectedMinPriceAmount,
                MaxPriceAmount = SelectedMaxPriceAmount,
                PriceCurrency = SelectedPriceCurrency
            };

            var result = await ProductAppService.GetListAsync(filter);
            Products = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ProductListItemDto> e)
        {
            PageSize = e.PageSize;
            CurrentPage = e.Page - 1;
            CurrentSorting = string.Join(",", e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => $"{c.Field} {(c.SortDirection == SortDirection.Descending ? "DESC" : "")}"));

            await GetDataAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task GetCompanyAsync()
        {
            Company = await CompanyAppService.GetCompanyLookupAsync();
        }

        protected Task View(Guid id)
        {
            NavigationManager.NavigateTo($"/product/{id}");
            return Task.CompletedTask;
        }

        protected override Task CreateAsync()
        {
            NavigationManager.NavigateTo("/management/product/new");
            return Task.CompletedTask;
        }

        protected override Task EditAsync(Guid id)
        {
            NavigationManager.NavigateTo($"/management/product/edit/{id}");
            return Task.CompletedTask;
        }

        protected override async Task DeleteAsync(Guid id)
        {
            var confirmMessage = L["Message:ProductDeletionConfirmation"];
            var message = confirmMessage;
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            await ProductAppService.DeleteAsync(id);
            await GetDataAsync();
        }

        protected async Task PublishAsync(Guid id)
        {
            var confirmMessage = L["Message:ProductPublicationConfirmation"];
            var message = confirmMessage;
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            await ProductAppService.PublishAsync(id);
            await GetDataAsync();
        }

        protected async Task UnpublishAsync(Guid id)
        {
            var confirmMessage = L["Message:ProductCancelPublicationConfirmation"];
            var message = confirmMessage;
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            await ProductAppService.UnpublishAsync(id);
            await GetDataAsync();
        }
    }
}