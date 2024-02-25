using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using WebMarketplace.Localization;

namespace WebMarketplace.Permissions;

public class WebMarketplacePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WebMarketplacePermissions.GroupName);
        var vendorManagementGroup = context.AddGroup(WebMarketplacePermissions.VendorManagementGroup,
            L("Permission:VendorsManagementGroup"));
        var productsManagementGroup = context.AddGroup(WebMarketplacePermissions.ProductsManagementGroup,
            L("Permission:ProductsManagementGroup"));
        var ordersManagementGroup = context.AddGroup(WebMarketplacePermissions.OrdersManagementGroup,
            L("Permission:OrdersManagementGroup"));
        var cartsManagementGroup = context.AddGroup(WebMarketplacePermissions.CartsManagementGroup,
            L("Permission:CartsManagementGroup"));
        var reviewsManagementGroup = context.AddGroup(WebMarketplacePermissions.ReviewsManagementGroup,
            L("Permission:ReviewsManagementGroup"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(WebMarketplacePermissions.MyPermission1, L("Permission:MyPermission1"));

        var vendorsPermission = vendorManagementGroup.AddPermission(
            WebMarketplacePermissions.Vendors.Default, L("Permission:Vendors"));
        vendorsPermission.AddChild(
            WebMarketplacePermissions.Vendors.Create, L("Permission:Vendors.Create"));
        vendorsPermission.AddChild(
            WebMarketplacePermissions.Vendors.Edit, L("Permission:Vendors.Edit"));
        vendorsPermission.AddChild(
            WebMarketplacePermissions.Vendors.Delete, L("Permission:Vendors.Delete"));

        var userVendorsPermission = vendorManagementGroup.AddPermission(
            WebMarketplacePermissions.UserVendors.Default, L("Permission:UserVendors"));
        userVendorsPermission.AddChild(
            WebMarketplacePermissions.UserVendors.Create, L("Permission:UserVendors.Create"));
        userVendorsPermission.AddChild(
            WebMarketplacePermissions.UserVendors.Edit, L("Permission:UserVendors.Edit"));
        userVendorsPermission.AddChild(
            WebMarketplacePermissions.UserVendors.Delete, L("Permission:UserVendors.Delete"));

        var productsPermission = productsManagementGroup.AddPermission(
            WebMarketplacePermissions.Products.Default, L("Permission:Products"));
        productsPermission.AddChild(
            WebMarketplacePermissions.Products.Create, L("Permission:Products.Create"));
        productsPermission.AddChild(
            WebMarketplacePermissions.Products.Edit, L("Permission:Products.Edit"));
        productsPermission.AddChild(
            WebMarketplacePermissions.Products.Delete, L("Permission:Products.Delete"));

        var productCategoriesPermission = productsManagementGroup.AddPermission(
            WebMarketplacePermissions.ProductCategories.Default, L("Permission:ProductCategories"));
        productCategoriesPermission.AddChild(
            WebMarketplacePermissions.ProductCategories.Create, L("Permission:ProductCategories.Create"));
        productCategoriesPermission.AddChild(
            WebMarketplacePermissions.ProductCategories.Edit, L("Permission:ProductCategories.Edit"));
        productCategoriesPermission.AddChild(
            WebMarketplacePermissions.ProductCategories.Delete, L("Permission:ProductCategories.Delete"));

        var ordersPermission = ordersManagementGroup.AddPermission(
            WebMarketplacePermissions.Orders.Default, L("Permission:Orders"));
        ordersPermission.AddChild(
            WebMarketplacePermissions.Orders.Create, L("Permission:Orders.Create"));
        ordersPermission.AddChild(
            WebMarketplacePermissions.Orders.Edit, L("Permission:Orders.Edit"));
        ordersPermission.AddChild(
            WebMarketplacePermissions.Orders.Delete, L("Permission:Orders.Delete"));

        var cartsPermission = cartsManagementGroup.AddPermission(
            WebMarketplacePermissions.Carts.Default, L("Permission:Carts"));
        cartsPermission.AddChild(
            WebMarketplacePermissions.Carts.Create, L("Permission:Carts.Create"));
        cartsPermission.AddChild(
            WebMarketplacePermissions.Carts.Edit, L("Permission:Carts.Edit"));
        cartsPermission.AddChild(
            WebMarketplacePermissions.Carts.Delete, L("Permission:Carts.Delete"));

        var reviewsPermission = reviewsManagementGroup.AddPermission(
            WebMarketplacePermissions.Reviews.Default, L("Permission:Reviews"));
        reviewsPermission.AddChild(
            WebMarketplacePermissions.Reviews.Create, L("Permission:Reviews.Create"));
        reviewsPermission.AddChild(
            WebMarketplacePermissions.Reviews.Edit, L("Permission:Reviews.Edit"));
        reviewsPermission.AddChild(
            WebMarketplacePermissions.Reviews.Delete, L("Permission:Reviews.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebMarketplaceResource>(name);
    }
}