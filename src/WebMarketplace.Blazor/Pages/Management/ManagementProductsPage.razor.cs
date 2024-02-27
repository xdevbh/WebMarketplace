using WebMarketplace.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using WebMarketplace.Products;


namespace WebMarketplace.Blazor.Pages.Management
{
    public partial class ManagementProductsPage
    {
        private IReadOnlyList<ProductDto> ProductList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }

        private bool CanCreateAuthor { get; set; }
        private bool CanEditAuthor { get; set; }
        private bool CanDeleteAuthor { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetProductsAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateAuthor = await AuthorizationService
                .IsGrantedAsync(WebMarketplacePermissions.Products.Create);

            CanEditAuthor = await AuthorizationService
                .IsGrantedAsync(WebMarketplacePermissions.Products.Edit);

            CanDeleteAuthor = await AuthorizationService
                .IsGrantedAsync(WebMarketplacePermissions.Products.Delete);
        }

        private async Task GetProductsAsync()
        {
            var result = await ProductAppService.GetListAsync(new PagedAndSortedResultRequestDto()
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting
            });

            ProductList = result.Items;
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
    }
}