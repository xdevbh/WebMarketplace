namespace WebMarketplace.Permissions;

public static class WebMarketplacePermissions
{
    public const string GroupName = "WebMarketplace";

    public static class Vendors
    {
        public const string Default = GroupName + ".Vendors";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    
    public static class Addresses
    {
        public const string Default = GroupName + ".Addresses";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Publish = Default + ".Publish";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
}