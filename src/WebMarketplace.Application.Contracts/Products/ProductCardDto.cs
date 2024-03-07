using System;
using OpenIddict.Abstractions;

namespace WebMarketplace.Products;

public class ProductCardDto
{
    public Guid Id { get; set; }
    
    public Guid VendorId { get; set; }
    public string VendorName { get; set; }
    
    public Guid? ProductCategotyId { get; set; }
    public string ProductCategoryName { get; set; }
    
    public string Name { get; set; }
    public double? Rating { get; set; }
    public string ShortDescription { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
    public int InStock { get; set; }
    
}