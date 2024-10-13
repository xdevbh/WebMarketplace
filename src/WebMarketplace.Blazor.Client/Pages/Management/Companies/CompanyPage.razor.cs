using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using WebMarketplace.Addresses;
using WebMarketplace.Blazor.Client.Components;
using WebMarketplace.Companies;
using WebMarketplace.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace WebMarketplace.Blazor.Client.Pages.Management.Companies;

public partial class CompanyPage
{
    protected WmAddressEdit WmAddressEditRef;
    protected CompanyDto Company { get; set; } = new();
    protected UpdateCompanySellerDto EditCompany { get; set; } = new();
    protected AddressDto Address { get; set; } = new();
    protected CreateUpdateAddressDto EditAddress { get; set; } = new();
    protected bool CanEdit { get; set; }
    protected Validations CompanyValidationsRef { get; set; }
    protected bool IsEditMode { get; set; }
    protected IReadOnlyList<CompanyImageDto> Images { get; set; } = new List<CompanyImageDto>();
    protected CompanyImageNewModal AddImageModalRef { get; set; }
    protected int ImageTotalCount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PageTitle = L["Page:Company"];
        PageHeader = L["Header:Company"];
        await GetDataAsync();
        await base.OnInitializedAsync();
    }

    protected override async Task SetPermissionsAsync()
    {
        CanEdit = await AuthorizationService.IsGrantedAsync(WebMarketplacePermissions.Companies.Update);
        await base.SetPermissionsAsync();
    }

    protected override Task SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:Management"], "/management"));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:Company"]));
        return base.SetBreadcrumbItemsAsync();
    }

    protected override async Task SetToolBarAsync()
    {
        if (!IsEditMode)
        {
            PageToolbar = new PageToolbar();
            PageToolbar.AddButton(L["Action:Edit"],
                () => EnableEditMode(true),
                IconName.Edit,
                Color.Primary,
                !CanEdit);
        }
        else
        {
            PageToolbar = new PageToolbar();
            PageToolbar.AddButton(L["Action:Cancel"],
                () => CancelAsync(),
                IconName.Times,
                Color.Danger);

            PageToolbar.AddButton(L["Action:Save"],
                async () => UpdateAsync(),
                IconName.Save,
                Color.Primary,
                !CanEdit);
        }
        
        await InvokeAsync(StateHasChanged);
        await base.SetToolBarAsync();
    }

    protected async Task UpdateAsync()
    {
        if (await WmAddressEditRef.ValidateAll() && await CompanyValidationsRef.ValidateAll())
        {
            var addressDto = await AddressAppService.UpdateAsync(Company.AddressId, EditAddress);

            Company.AddressId = addressDto.Id;
            // Company.FullDescription = // todo: full description rich text editor

            await CompanyAppService.UpdateAsync(EditCompany);
            await EnableEditMode(false);
        }
    }

    protected async Task EnableEditMode(bool enable)
    {
        IsEditMode = enable;
        await SetToolBarAsync();
    }
    
    protected Task CancelAsync()
    {
        EnableEditMode(false);
        return Task.CompletedTask;
    }

    protected async Task GetDataAsync()
    {
        Company = await CompanyAppService.GetAsync();
        EditCompany = ObjectMapper.Map<CompanyDto, UpdateCompanySellerDto>(Company);
        Address = await AddressAppService.GetAsync(Company.AddressId);
        EditAddress = ObjectMapper.Map<AddressDto, CreateUpdateAddressDto>(Address);
    }

    #region Images

    protected async Task GetImagesAsync()
    {
        var result = await CompanyAppService.GetAllImagesAsync();
        Images = result.Items;
        ImageTotalCount = result.Items.Count;
    }

    protected async Task OnImagesDataGridReadAsync(DataGridReadDataEventArgs<CompanyImageDto> e)
    {
        await GetImagesAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected async Task AddImageAsync()
    {
        await AddImageModalRef.Show();
    }

    protected async Task SetDefaultAsync(string blobName)
    {
        var updateDto = new UpdateCompanyImageDto
        {
            BlobName = blobName,
            IsDefault = true
        };

        await CompanyAppService.SetDefaultImageAsync(updateDto); // todo: fix
        await GetImagesAsync();
    }

    protected async Task DeleteImagesAsync(string blobName)
    {
        var confirmMessage = L["Message:CompanyImageDeletionConfirmation"];
        var message = confirmMessage;
        if (!await UiMessageService.Confirm(message))
        {
            return;
        }

        await CompanyAppService.DeleteImageAsync(blobName);
        await GetImagesAsync();
    }

    #endregion
}