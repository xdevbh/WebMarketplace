using WebMarketplace.Vendors;

namespace WebMarketplace.Products;

public class ProductVendorQueryResultItem
{
    public Product Product { get; set; }
    public Vendor Vendor { get; set; }

    public ProductVendorQueryResultItem(Product product, Vendor vendor)
    {
        Product = product;
        Vendor = vendor;
    }
}