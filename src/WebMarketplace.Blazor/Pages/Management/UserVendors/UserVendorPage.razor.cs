using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.Application.Dtos;
using WebMarketplace.Permissions;
using WebMarketplace.Vendors;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Identity;
using WebMarketplace.UserVendors;


namespace WebMarketplace.Blazor.Pages.Management.UserVendors;

public partial class UserVendorPage
{
    private IReadOnlyList<UserVendorDto> UserVendors = new List<UserVendorDto>();
    private IReadOnlyList<VendorDto> Vendors = new List<VendorDto>();
    private IReadOnlyList<IdentityUserDto> Users = new List<IdentityUserDto>();
    private IFluentSpacingOnBreakpointWithSideAndSize commonMargin = Margin.Is3.FromBottom;
    private UserVendorDto selectedUserVendor;
    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }
    private bool CanCreateUserVendor { get; set; }
    private bool CanEditUserVendor { get; set; }
    private bool CanDeleteUserVendor { get; set; }

    private Guid selectedUserFilter = Guid.Empty;
    private Guid selectedVendorFilter = Guid.Empty;

    private CreateUpdateUserVendorDto NewUserVendor { get; set; } = new CreateUpdateUserVendorDto();
    private CreateUpdateUserVendorDto EditUserVendor { get; set; } = new CreateUpdateUserVendorDto();

    private Modal CreateUserVendorModal { get; set; }
    private Modal EditUserVendorModal { get; set; }

    private Validations CreateValidationsRef;
    private Validations EditValidationsRef;

    private void OpenCreateUserVendorModal()
    {
        CreateValidationsRef.ClearAll();
        NewUserVendor = new CreateUpdateUserVendorDto();
        CreateUserVendorModal.Show();
    }

    private void CloseCreateUserVendorModal()
    {
        CreateUserVendorModal.Hide();
    }

    private void OpenEditUserVendorModal(UserVendorDto userVendorDto)
    {
        EditValidationsRef.ClearAll();
        EditUserVendor = new CreateUpdateUserVendorDto();
        EditUserVendor = ObjectMapper.Map<UserVendorDto, CreateUpdateUserVendorDto>(userVendorDto);
        EditUserVendorModal.Show();
    }

    private void CloseEditUserVendorModal()
    {
        EditUserVendorModal.Hide();
    }

    private async Task CreateUserVendorAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await UserVendorAppService.CreateAsync(NewUserVendor);
            await GetUserVendorsAsync();
            CreateUserVendorModal.Hide();
        }
    }

    private async Task UpdateUserVendorAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await UserVendorAppService.UpdateAsync(EditUserVendor.Id.Value, EditUserVendor);
            await GetUserVendorsAsync();
            EditUserVendorModal.Hide();
        }
    }

    private async Task DeleteUserVendorAsync(UserVendorDto userVendorDto)
    {
        var confirmMessage = L["UserVendorDeletionConfirmationMessage"];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }

        await UserVendorAppService.DeleteAsync(userVendorDto.Id);
        await GetUserVendorsAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetUserVendorsAsync();
        await GetUsersAsync();
        await GetVendorsAsync();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateUserVendor = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.UserVendors.Create);

        CanEditUserVendor = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.UserVendors.Edit);

        CanDeleteUserVendor = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.UserVendors.Delete);
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<UserVendorDto> e)
    {
        CurrentPage = e.Page - 1;
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        

        await GetUserVendorsAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task GetUserVendorsAsync()
    {
        var result = await UserVendorAppService.GetFilteredListAsync(
            new UserVendorRequestDto()
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting,
                UserId = selectedUserFilter == Guid.Empty ? null : selectedUserFilter,
                VendorId = selectedVendorFilter == Guid.Empty ? null : selectedVendorFilter
            }
        );

        UserVendors = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task GetUsersAsync()
    {
        var result = await UserVendorAppService.GetAllUsersAsync();
        Users = result;
    }

    private async Task GetVendorsAsync()
    {
        var result = await UserVendorAppService.GetAllVendorsAsync();
        Vendors = result;
    }
}