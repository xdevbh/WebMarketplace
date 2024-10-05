using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebMarketplace.Localization;
using WebMarketplace.Permissions;
using WebMarketplace.MultiTenancy;
using Volo.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.Users;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.Identity.Blazor;

namespace WebMarketplace.Blazor.Client.Navigation;

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

    #region Main Menu
    
    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();
        
        await ConfigureHomeMenuAsync(context);
        await ConfigureProductsMenuAsync(context);
        await ConfigureCompaniesMenuAsync(context);
        await ConfigureManagementMenuAsync(context);

        //Administration
        await ConfigureAdministrationMenuAsync(context);
    }

    private static async Task ConfigureHomeMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            WebMarketplaceMenus.Home,
            l["Menu:Home"],
            "/",
            icon: "fas fa-home",
            order: 1
        ));
    }
    
    private static async Task ConfigureProductsMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            WebMarketplaceMenus.Products,
            l["Menu:Products"],
            "/product/list",
            icon: "fa fa-book",
            order: 1
        ));
    }
    
    private static async Task ConfigureCompaniesMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            WebMarketplaceMenus.Companies,
            l["Menu:Companies"],
            "/company/list",
            icon: "fa fa-building",
            order: 1
        ));
    }
    
    private static async Task ConfigureManagementMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            WebMarketplaceMenus.Management,
            l["Menu:Management"],
            "/management/",
            icon: "fa fa-cog",
            order: 1
        ));
    }

    private static async Task ConfigureAdministrationMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        var administration = context.Menu.GetAdministration();
        administration.Order = 5;
        
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
    }
    
    #endregion

    #region User Menu

    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
            icon: "fa fa-user-circle-o",
            order: 1000,
            target: "_blank").RequireAuthenticated());

        await Task.CompletedTask;
    }
    
    
    #endregion
}
