using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Net.Http.Headers;
using NUglify.Helpers;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.BlazoriseUI;

namespace WebMarketplace.Blazor.Client.Components;

public partial class WmPageHeader
{
    protected List<RenderFragment> ToolbarItemRenders { get; set; }

    public IPageToolbarManager PageToolbarManager { get; set; } = default!;

    [Parameter]
    public string? Title
    {
        get => PageLayout.Title;
        set => PageLayout.Title = value;
    }

    [Parameter] public string? Header { get; set; } = null;
    
    [Parameter] public bool BreadcrumbShowHome { get; set; } = true;

    [Parameter] public bool BreadcrumbShowCurrent { get; set; } = true;

    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    [Parameter] // TODO: Consider removing this property in future and use only PageLayout.
    public List<BreadcrumbItem> BreadcrumbItems
    {
        get => PageLayout.BreadcrumbItems.ToList();
        set
        {
            PageLayout.BreadcrumbItems.Clear();
            foreach (var item in value)
            {
                PageLayout.BreadcrumbItems.Add(item);
            }
        }
    }

    [Parameter] public PageToolbar? Toolbar { get; set; }

    public WmPageHeader()
    {
        ToolbarItemRenders = new List<RenderFragment>();
    }

    protected async override Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (Toolbar != null)
        {
            var toolbarItems = await PageToolbarManager.GetItemsAsync(Toolbar);

            if (!ShouldRenderToolbarItems(toolbarItems))
            {
                return;
            }

            ToolbarItemRenders.Clear();

            if (!Options.Value.RenderToolbar)
            {
                PageLayout.ToolbarItems.Clear();
                foreach (var item in toolbarItems)
                {
                    PageLayout.ToolbarItems.Add(item);
                }

                return;
            }

            foreach (var item in toolbarItems)
            {
                var sequence = 0;
                ToolbarItemRenders.Add(builder =>
                {
                    builder.OpenComponent(sequence, item.ComponentType);
                    if (item.Arguments != null)
                    {
                        foreach (var argument in item.Arguments)
                        {
                            sequence++;
                            builder.AddAttribute(sequence, argument.Key, argument.Value);
                        }
                    }

                    builder.CloseComponent();
                });
            }
        }

        await FixTitle();
        await FixHeader();
    }

    protected async Task FixTitle()
    {
        if (AbpStringExtensions.IsNullOrWhiteSpace(Title))
        {
            Title = BrandingProvider.AppName;
        }
        else
        {
            Title += " | " + BrandingProvider.AppName;
        }
    }

    protected async Task FixHeader()
    {
        if (AbpStringExtensions.IsNullOrWhiteSpace(Header))
        {
            if (NUglifyExtensions.IsNullOrWhiteSpace(Title))
            {
                await FixTitle();
            }

            Header = Title;
        }
    }

    protected virtual bool ShouldRenderToolbarItems(PageToolbarItem[] items)
    {
        if (items.Length != PageLayout.ToolbarItems.Count)
        {
            return true;
        }

        return items.Where((t, i) => t.ComponentType != PageLayout.ToolbarItems[i].ComponentType).Any();
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

}