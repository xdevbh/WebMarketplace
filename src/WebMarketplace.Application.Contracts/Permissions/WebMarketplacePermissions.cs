namespace WebMarketplace.Permissions;

public static class WebMarketplacePermissions
{
    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public const string GroupName = "WebMarketplace";
    public const string VendorManagementGroup = "VendorManagement";
    public const string ProductsManagementGroup = "ProductManagement";
    public const string OrdersManagementGroup = "OrderManagement";
    public const string CartsManagementGroup = "CartManagement";
    public const string ReviewsManagementGroup = "ReviewsManagement";

    public static class Vendors
    {
        public const string Default = VendorManagementGroup + ".Vendors";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class UserVendors
    {
        public const string Default = VendorManagementGroup + ".UserVendors";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Products
    {
        public const string Default = ProductsManagementGroup + ".Products";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class ProductCategories
    {
        public const string Default = ProductsManagementGroup + ".ProductCategories";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Orders // Orders and OrderItems
    {
        public const string Default = OrdersManagementGroup + ".Orders";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Carts // Carts and CartItems
    {
        public const string Default = CartsManagementGroup + ".Carts";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Reviews
    {
        public const string Default = ReviewsManagementGroup + ".Reviews";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}