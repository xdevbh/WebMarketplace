using System;
using System.Threading.Tasks;
using Blazorise;
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
            icon: "fa fa-shapes",
            order: 2
        ));
    }

    private static async Task ConfigureCompaniesMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            WebMarketplaceMenus.Companies,
            l["Menu:Companies"],
            "/company/list",
            icon: "fa fa-store",
            order: 3
        ));
    }

    private static async Task ConfigureManagementMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        var management = new ApplicationMenuItem(
            WebMarketplaceMenus.Management,
            l["Menu:Management"],
            "/management",
            "fa fa-cogs",
            4
        ).RequireAuthenticated().RequirePermissions("SellerOnly");
        context.Menu.AddItem(management);

        var managementDashboard = new ApplicationMenuItem(
            WebMarketplaceMenus.ManagementProduct,
            l["Menu:Dashboard"],
            "/management",
            icon: "fa fa-table-columns",
            order: 1
        );

        var managementOrders = new ApplicationMenuItem(
            WebMarketplaceMenus.ManagementOrder,
            l["Menu:Orders"],
            "/management/order/list",
            icon: "fa fa-list",
            order: 2
        );
        management.AddItem(managementOrders);

        var managementCustomers = new ApplicationMenuItem(
            WebMarketplaceMenus.ManagementOrder,
            l["Menu:Customers"],
            "/management/customer/list",
            icon: "fa fa-users",
            order: 3
        );
        management.AddItem(managementCustomers);

        var managementProducts = new ApplicationMenuItem(
            WebMarketplaceMenus.ManagementProduct,
            l["Menu:Products"],
            "/management/product/list",
            icon: "fa fa-shapes",
            order: 4
        );
        management.AddItem(managementProducts);
        
        var managementBlogPosts = new ApplicationMenuItem(
            WebMarketplaceMenus.ManagementProduct,
            l["Menu:BlogPosts"],
            "/management/blogpost/list",
            icon: "fa fa-message",
            order: 5
        );
        management.AddItem(managementBlogPosts);


        var managementCompanies = new ApplicationMenuItem(
            WebMarketplaceMenus.ManagementCompany,
            l["Menu:Company"],
            "/management/company",
            icon: "fa fa-building",
            order: 6
        );
        management.AddItem(managementCompanies);
    }

    private static async Task ConfigureAdministrationMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();

        var administration = context.Menu.GetAdministration().RequireAuthenticated(); // todo: admin only
        administration.Order = 6;

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
        
        var administrationCompanies = new ApplicationMenuItem(
            WebMarketplaceMenus.AdministrationCompanies,
            l["Menu:Companies"],
            "/administration/company/list",
            icon: "fa fa-store",
            order: 4
        ).RequireAuthenticated().RequirePermissions("AdminOnly");
        administration.AddItem(administrationCompanies);
    }

    #endregion

    #region User Menu

    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<WebMarketplaceResource>();
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            WebMarketplaceMenus.Orders,
            l["Menu:MyOrders"],
            $"/account/order/list",
            icon: "fa fa-list",
            order: 1).RequireAuthenticated());
        
        context.Menu.AddItem(new ApplicationMenuItem(
            WebMarketplaceMenus.Orders,
            l["Menu:MyAddresses"],
            $"/account/address/list",
            icon: "fa fa-map-location-dot",
            order: 1).RequireAuthenticated());

        context.Menu.AddItem(new ApplicationMenuItem(
            WebMarketplaceMenus.AccountManage,
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
            icon: "fa fa-cog",     
            order: 1000,
            target: "_blank").RequireAuthenticated());

        await Task.CompletedTask;
    }

    #endregion
}