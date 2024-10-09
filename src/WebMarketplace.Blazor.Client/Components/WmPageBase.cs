using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;
using WebMarketplace.Permissions;

namespace WebMarketplace.Blazor.Client.Components;

public abstract class WmPageBase : AbpComponentBase
{
    protected string PageTitle { get; set; } = string.Empty;
    protected string PageHeader { get; set; } = string.Empty;
    protected PageToolbar PageToolbar { get; set; } = new();
    protected List<BreadcrumbItem> BreadcrumbItems { get; set; } = new();

    public WmPageBase()
    {
        PageToolbar = new();
        BreadcrumbItems = new();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await SetToolBarAsync();
        await SetBreadcrumbItemsAsync();
        await base.OnInitializedAsync();
    }

    protected virtual Task SetToolBarAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task SetBreadcrumbItemsAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task SetPermissionsAsync()
    {
        return Task.CompletedTask;
    }
}