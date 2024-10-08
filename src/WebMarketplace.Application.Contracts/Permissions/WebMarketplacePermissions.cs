namespace WebMarketplace.Permissions;

public static class WebMarketplacePermissions
{
    public const string GroupName = "WebMarketplace";

    public static class Companies
    {
        public const string Default = GroupName + ".Companies";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    
    public static class CompanyMemberships
    {
        public const string Default = GroupName + ".CompanyMemberships";
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
    
    public static class Orders
    {
        public const string Default = GroupName + ".Orders";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string ChangeStatus = Default + ".ChangeStatus";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
}