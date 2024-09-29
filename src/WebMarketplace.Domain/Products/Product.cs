using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using WebMarketplace.Products.ProductReviews;

namespace WebMarketplace.Products;

public class Product : AuditedAggregateRoot<Guid>
{
    public Guid VendorId { get; set; }
    
    public string Name { get; set; }
    
    public ProductCategory ProductCategory { get; set; }
    
    public ProductType ProductType { get; set; }
    
    public string? ShortDescription { get; set; }
    
    public string? FullDescription { get; set; }
    
    public List<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();    
    
    // todo: to another tables
    public double Price { get; set; }
    public string Currency { get; set; }
    public double TaxPercent { get; set; }
    public double? AverageRating { get; set; } // fill with OnReviewCreated event
    public int InStock { get; set; }
    public bool IsPublished { get; set; }
    public string? ImagePath { get; set; }
}