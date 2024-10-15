using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using WebMarketplace.Blazor.Client.Components;

namespace WebMarketplace.Blazor.Client.Navigation;

public class WebMarketplaceToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            // todo: redirect to login page if user is not authenticated
            context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(WmSearchToolBarItem)));
            context.Toolbar.Items.Insert(1, new ToolbarItem(typeof(WmCartToolBarItem)));
        }

        return Task.CompletedTask;
    }
}