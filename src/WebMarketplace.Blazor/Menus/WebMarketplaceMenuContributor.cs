using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMarketplace.Localization;
using WebMarketplace.MultiTenancy;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace WebMarketplace.Blazor.Menus;

public class WebMarketplaceMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public WebMarketplaceMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                WebMarketplaceMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home"
            )
        );
        
        var productsMenu = new ApplicationMenuItem(
            WebMarketplaceMenus.Products,
            l["Menu:Products"],
            "/products",
            icon: "fa fa-book"
        );
        context.Menu.Items.Insert(1, productsMenu);

        var organizationsMenu = new ApplicationMenuItem(
            WebMarketplaceMenus.Organizations,
            l["Menu:Organizations"],
            "/organizations",
            icon: "fa fa-building"
        );
        context.Menu.Items.Insert(2,organizationsMenu);


        var managementMenu = new ApplicationMenuItem(
            WebMarketplaceMenus.Management,
            l["Menu:Management"],
            "/management",
            icon: "fa fa-cog"
        );
        context.Menu.AddItem(managementMenu);
     
        var administration = context.Menu.GetAdministration();

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();

        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-cog",
            order: 1000,
            null).RequireAuthenticated());

        return Task.CompletedTask;
    }
}
