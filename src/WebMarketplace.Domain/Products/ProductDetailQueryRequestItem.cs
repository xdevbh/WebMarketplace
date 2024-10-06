using System;
using System.Collections.Generic;

namespace WebMarketplace.Products;

public class ProductDetailQueryRequestItem
{
    public Guid CompanyId { get; set; }
    
    public String CompanyName { get; set; }

    public string Name { get; private set; }

    public ProductCategory ProductCategory { get; set; }

    public ProductType ProductType { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public bool IsPublished { get; private set; }
    
    public double Rating { get; set; }
    
    public ProductPrice Price { get; set; }
    
    public virtual List<ProductReview> Reviews { get; set; }
    
    public List<ProductImage> Images { get; set; }
}