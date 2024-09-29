using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Products.ProductReviews;

public class ProductReview : Entity<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public double Rating { get; set; }
    public string? Comment { get; set; }
}