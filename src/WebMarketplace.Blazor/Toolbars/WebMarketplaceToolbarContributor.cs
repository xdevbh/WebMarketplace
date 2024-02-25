using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using WebMarketplace.Blazor.Components;

namespace WebMarketplace.Blazor.Toolbars;

public class WebMarketplaceToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(ToolbarSearch)));
            context.Toolbar.Items.Insert(1, new ToolbarItem(typeof(ToolbarCart)));
        }

        return Task.CompletedTask;
    }
}