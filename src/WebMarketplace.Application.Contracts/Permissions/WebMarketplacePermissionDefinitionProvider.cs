using WebMarketplace.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace WebMarketplace.Permissions;

public class WebMarketplacePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WebMarketplacePermissions.GroupName, L("Permission:WebMarketplace"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(WebMarketplacePermissions.MyPermission1, L("Permission:MyPermission1"));
        
        var companiesPermission = myGroup.AddPermission(WebMarketplacePermissions.Companies.Default, L("Permission:Companies"));
        companiesPermission.AddChild(WebMarketplacePermissions.Companies.Create, L("Permission:Create"));
        companiesPermission.AddChild(WebMarketplacePermissions.Companies.Update, L("Permission:Update"));
        companiesPermission.AddChild(WebMarketplacePermissions.Companies.Delete, L("Permission:Delete"));
        
        var companyMembershipsPermission = myGroup.AddPermission(WebMarketplacePermissions.CompanyMemberships.Default, L("Permission:CompanyMemberships"));
        companyMembershipsPermission.AddChild(WebMarketplacePermissions.CompanyMemberships.Create, L("Permission:Create"));
        companyMembershipsPermission.AddChild(WebMarketplacePermissions.CompanyMemberships.Update, L("Permission:Update"));
        companyMembershipsPermission.AddChild(WebMarketplacePermissions.CompanyMemberships.Delete, L("Permission:Delete"));
        
        var addressesPermission = myGroup.AddPermission(WebMarketplacePermissions.Addresses.Default, L("Permission:Addresses"));
        addressesPermission.AddChild(WebMarketplacePermissions.Addresses.Create, L("Permission:Create"));
        addressesPermission.AddChild(WebMarketplacePermissions.Addresses.Update, L("Permission:Update"));
        addressesPermission.AddChild(WebMarketplacePermissions.Addresses.Delete, L("Permission:Delete"));
        
        var productsPermission = myGroup.AddPermission(WebMarketplacePermissions.Products.Default, L("Permission:Products"));
        productsPermission.AddChild(WebMarketplacePermissions.Products.Create, L("Permission:Create"));
        productsPermission.AddChild(WebMarketplacePermissions.Products.Update, L("Permission:Update"));
        productsPermission.AddChild(WebMarketplacePermissions.Products.Delete, L("Permission:Delete"));
        productsPermission.AddChild(WebMarketplacePermissions.Products.Publish, L("Permission:Publish"));
        
        var ordersPermission = myGroup.AddPermission(WebMarketplacePermissions.Orders.Default, L("Permission:Orders"));
        ordersPermission.AddChild(WebMarketplacePermissions.Orders.Create, L("Permission:Create"));
        ordersPermission.AddChild(WebMarketplacePermissions.Orders.Update, L("Permission:Update"));
        ordersPermission.AddChild(WebMarketplacePermissions.Orders.Delete, L("Permission:Delete"));
        ordersPermission.AddChild(WebMarketplacePermissions.Orders.ChangeStatus, L("Permission:ChangeStatus"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebMarketplaceResource>(name);
    }
}