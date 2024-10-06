using System;
using Volo.Abp.Auditing;

namespace WebMarketplace.Products;

public class ProductReviewDetailQueryResultItem : IHasCreationTime
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    
    public double Rating { get; set; }

    public string? Comment { get; set; }
    
    public DateTime CreationTime { get; set; }
}