using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace WebMarketplace.Products;

public class ProductReview : AuditedEntity<Guid>
{
    public virtual Guid UserId { get; set; }
    // public virtual IdentityUser User { get; set; } 
    public virtual Guid ProductId { get; set; }
    public virtual double Rating { get; private set; }

    public virtual string? Comment { get; set; }

    protected ProductReview()
    {
    }

    public ProductReview(
        Guid id,
        Guid userId,
        Guid productId,
        double rating,
        string? comment)
        : base(id)
    {
        UserId = userId;
        ProductId = productId;
        SetRating(rating);
        Comment = comment;
    }

    public virtual ProductReview SetRating(double rating)
    {
        Check.Range(rating, nameof(rating), WebMarketplaceConsts.RatingMinValue, WebMarketplaceConsts.RatingMaxValue);
        
        Rating = rating;
        return this;
    }
}