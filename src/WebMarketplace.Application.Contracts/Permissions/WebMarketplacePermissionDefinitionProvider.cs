using WebMarketplace.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace WebMarketplace.Permissions;

public class WebMarketplacePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WebMarketplacePermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(WebMarketplacePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebMarketplaceResource>(name);
    }
}
