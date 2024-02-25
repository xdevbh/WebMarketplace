namespace WebMarketplace.Blazor.Menus;

public class WebMarketplaceMenus
{
    private const string Prefix = "WebMarketplace";
    public const string Home = Prefix + ".Home";

    //Add your menu items here...
    public const string Products = Prefix + ".Products";
    public const string Vendors = Prefix + ".Vendors";
    
    public const string Management = Prefix + ".Management";
    public const string ProductManagement = Management + ".Product";
    public const string VendorManagement = Management + ".Vendor";
    public const string OrderManagement = Management + ".Order";

}
