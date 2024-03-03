using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using WebMarketplace.Permissions;
using WebMarketplace.Vendors;

namespace WebMarketplace.Blazor.Pages.Management.Vendors;

public partial class ManagementVendorListPage
{
    private IReadOnlyList<VendorDto> Vendors = new List<VendorDto>();
    private IReadOnlyList<VendorDto> AllVendors = new List<VendorDto>();

    private VendorDto selectedVendor;
    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }
    private bool CanCreateVendor { get; set; }
    private bool CanEditVendor { get; set; }
    private bool CanDeleteVendor { get; set; }
    private CreateUpdateVendorDto NewVendor { get; set; } = new CreateUpdateVendorDto();
    private Modal CreateModal { get; set; }
    private Validations CreateValidationsRef;
    private string selectedNameFilter = string.Empty;


    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetAllVendorsAsync();
    }

    private async Task GetAllVendorsAsync()
    {
        var result = await VendorAppService.GetAllVendorsAsync();
        AllVendors = result.Items;
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateVendor = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Vendors.Create);

        CanEditVendor = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Vendors.Edit);

        CanDeleteVendor = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Vendors.Delete);
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<VendorDto> e)
    {
        CurrentPage = e.Page - 1;
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");


        await GetVendorsAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task GetVendorsAsync()
    {
        var result = await VendorAppService.GetFilteredListAsync(
            new VendorRequestDto()
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting,
                Name = selectedNameFilter
            }
        );

        Vendors = result.Items;
        TotalCount = (int)result.TotalCount;
    }


    private async Task DeleteUserVendorAsync(VendorDto context)
    {
        var confirmMessage = L["VendorDeletionConfirmationMessage"];
        var message =confirmMessage + " '" + context.Name + "'?";
        if (!await Message.Confirm(message))
        {
            return;
        }

        await VendorAppService.DeleteAsync(context.Id);
        await GetVendorsAsync();
    }

    private void OpenEditVendor(VendorDto context)
    {
        NavigationManager.NavigateTo($"/management/vendor/edit/{context.Id}");
    }

    private void OpenCreateUserVendorModal()
    {
        CreateValidationsRef.ClearAll();
        NewVendor = new CreateUpdateVendorDto();
        CreateModal.Show();
    }
    
    private void CloseCreateUserVendorModal()
    {
        CreateModal.Hide();
    }
    
    private async Task CreateVendorAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await VendorAppService.CreateAsync(NewVendor);
            await GetVendorsAsync();
            CreateModal.Hide();
        }
    }
}