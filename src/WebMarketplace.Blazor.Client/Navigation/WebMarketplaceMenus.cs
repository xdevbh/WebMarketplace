namespace WebMarketplace.Blazor.Client.Navigation;

public class WebMarketplaceMenus
{
    private const string Prefix = "WebMarketplace";

    public const string Home = Prefix + ".Home";
    
    public const string Products = Prefix + ".Products";
    
    public const string Companies = Prefix + ".Companies";
    
    public const string Management = Prefix + ".Management";
    public const string ManagementCompany = Management + ".Company";
    public const string ManagementOrder = Management + ".Orders";
    public const string ManagementOrderList = ManagementOrder + ".List";
    public const string ManagementOrderEdit = ManagementOrder + ".Edit";
    public const string ManagementProduct = Management + ".Products";
    public const string ManagementProductList = ManagementProduct + ".List";
    public const string ManagementProductEdit = ManagementProduct + ".Edit";
    
    public const string Administration = Prefix + ".Administration";
    public const string AdministrationCompanies = Administration + ".Companies";
    
    private const string Account = "Account";
    public const string Orders = Account + ".Orders";
    public const string Addresses = Account + ".Addresses";
    public const string AccountManage = Account + ".Manage";
}