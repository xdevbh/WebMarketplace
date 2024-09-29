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
        
        var vendorsPermission = myGroup.AddPermission(WebMarketplacePermissions.Vendors.Default, L("Permission:Vendors"));
        vendorsPermission.AddChild(WebMarketplacePermissions.Vendors.Create, L("Permission:Create"));
        vendorsPermission.AddChild(WebMarketplacePermissions.Vendors.Update, L("Permission:Update"));
        vendorsPermission.AddChild(WebMarketplacePermissions.Vendors.Delete, L("Permission:Delete"));
        
        var addressesPermission = myGroup.AddPermission(WebMarketplacePermissions.Addresses.Default, L("Permission:Addresses"));
        addressesPermission.AddChild(WebMarketplacePermissions.Addresses.Create, L("Permission:Create"));
        addressesPermission.AddChild(WebMarketplacePermissions.Addresses.Update, L("Permission:Update"));
        addressesPermission.AddChild(WebMarketplacePermissions.Addresses.Delete, L("Permission:Delete"));
        
        
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebMarketplaceResource>(name);
    }
}