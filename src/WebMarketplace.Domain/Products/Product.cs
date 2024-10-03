using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Products;

public class Product : AuditedAggregateRoot<Guid>
{
    public virtual Guid VendorId { get; set; }

    public virtual string Name { get; private set; }

    public virtual ProductCategory ProductCategory { get; set; }

    public virtual ProductType ProductType { get; set; }

    public virtual string? ShortDescription { get; set; }

    public virtual string? FullDescription { get; set; }

    public virtual List<ProductReview> ProductReviews { get; set; }
    
    public virtual double Rating => ProductReviews.Average(x => x.Rating);

    public virtual List<ProductPrice> ProductPrices { get; set; }

    public virtual ProductPrice? ProductPrice => ProductPrices.OrderByDescending(x => x.Date).FirstOrDefault();

    public virtual bool IsPublished { get; private set; }


    protected Product()
    {
    }

    public Product(
        Guid id,
        Guid vendorId,
        string name,
        ProductCategory productCategory,
        ProductType productType,
        string? shortDescription,
        string? fullDescription)
        : base(id)
    {
        VendorId = vendorId;
        SetName(name);
        ProductCategory = productCategory;
        ProductType = productType;
        ShortDescription = shortDescription;
        FullDescription = fullDescription;
        IsPublished = false;

        ProductPrices = new List<ProductPrice>();
        ProductReviews = new List<ProductReview>();
    }

    public Product SetName(string name)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        return this;
    }

    public Product Publish(bool isPublish)
    {
        if (ProductPrices.Any() && isPublish)
        {
            IsPublished = isPublish;
        }
        else
        {
            IsPublished = false;
        }

        return this;
    }

    #region Reviews

    public Product AddReview(
        Guid reviewId,
        Guid userId,
        double rating,
        string? comment)
    {
        if (ProductReviews.Any(x => x.UserId == userId))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductReviewUserAlreadyExists);
        }

        var review = new ProductReview(reviewId, userId, this.Id, rating, comment);
        ProductReviews.Add(review);
        return this;
    }

    public Product UpdateReview(
        Guid reviewId,
        double rating,
        string? comment)
    {
        var review = ProductReviews.FirstOrDefault(x => x.Id == reviewId);
        if (review is null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductReviewNotFound);
        }

        review.SetRating(rating);
        review.Comment = comment;
        return this;
    }

    public Product RemoveReview(Guid reviewId)
    {
        var review = ProductReviews.FirstOrDefault(x => x.CreatorId == reviewId);
        if (review is null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductReviewNotFound);
        }

        ProductReviews.Remove(review);
        return this;
    }

    #endregion

    #region Prices

    public Product AddPrice(
        DateTime date,
        decimal amount,
        string currency)
    {
        if (ProductPrices.Any(x => x.Date == date))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductReviewUserAlreadyExists);
        }

        var price = new ProductPrice(this.Id, date, amount, currency);
        ProductPrices.Add(price);
        return this;
    }

    #endregion
}