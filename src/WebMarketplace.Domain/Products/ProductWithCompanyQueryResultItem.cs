using WebMarketplace.Companies;

namespace WebMarketplace.Products;

public class ProductWithCompanyQueryResultItem
{
    public Product Product { get; set; }
    public Company Company { get; set; }

    public ProductWithCompanyQueryResultItem(Product product, Company company)
    {
        Product = product;
        Company = company;
    }
}