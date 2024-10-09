using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using WebMarketplace.Companies;
using WebMarketplace.Permissions;

namespace WebMarketplace.Blazor.Client.Pages.Administration.Companies;

public partial class AdministrationCompanyListPage
{
    private string PageTitle { get; set; } = string.Empty;
    private bool CanCreateCompany { get; set; }
    private bool CanEditCompany { get; set; }
    private bool CanDeleteCompany { get; set; }
    private int PageSize { get; set; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }
    private IReadOnlyList<CompanyDto> Companies { get; set; } = new List<CompanyDto>();
    private string SelectedId { get; set; }
    private string SelectedIdentificationNumber { get; set; }
    private string SelectedName { get; set; }
    private string SelectedDisplayName { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetCompaniesAsync();
        
        PageTitle = L["Page:AdministrationCompanies"];
        base.OnInitializedAsync();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateCompany = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Companies.Create);

        CanEditCompany = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Companies.Update);

        CanDeleteCompany = await AuthorizationService
            .IsGrantedAsync(WebMarketplacePermissions.Companies.Delete);
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CompanyDto> e)
    {
        PageSize = e.PageSize;
        CurrentPage = e.Page - 1;
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");


        await GetCompaniesAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task GetCompaniesAsync()
    {
        Guid? id = null;
        if (Guid.TryParse(SelectedId, out var parsedId))
            id = parsedId;

        var filter = new CompanyFilterDto()
        {
            MaxResultCount = PageSize,
            SkipCount = CurrentPage * PageSize,
            Sorting = CurrentSorting,
            Id = id,
            CompanyIdentificationNumber = SelectedIdentificationNumber,
            Name = SelectedName,
            DisplayName = SelectedDisplayName
        };

        var result = await CompanyAdminAppService.GetListAsync(filter);

        Companies = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private void OpenEdit(CompanyDto context)
    {
        NavigationManager.NavigateTo($"/administration/company/edit/{context.Id}");
    }

    private void OpenCreate()
    {
        NavigationManager.NavigateTo($"/administration/company/new");
    }

    private async Task DeleteAsync(CompanyDto context)
    {
        var confirmMessage = L["Message:CompanyDeletionConfirmation"];
        var message = confirmMessage;
        if (!await UiMessageService.Confirm(message))
        {
            return;
        }

        await CompanyAdminAppService.DeleteAsync(context.Id);
        await GetCompaniesAsync();
    }
    
    private void OpenMembership(CompanyDto context)
    {
        NavigationManager.NavigateTo($"/administration/company/{context.Id}/membership/list/");
    }
}