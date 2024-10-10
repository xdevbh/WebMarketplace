using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Products;

public class Product : AuditedAggregateRoot<Guid>
{
    public virtual Guid CompanyId { get; set; }

    public virtual string Name { get; private set; }

    public virtual ProductCategory ProductCategory { get; set; }

    public virtual ProductType ProductType { get; set; }

    public virtual string? ShortDescription { get; set; }

    public virtual string? FullDescription { get; set; }
    
    public virtual bool IsPublished { get; private set; }

    public virtual List<ProductReview> Reviews { get; set; }

    public virtual double Rating => Reviews.Any() ? Reviews.Average(x => x.Rating) : 0;

    public virtual List<ProductPrice> Prices { get; set; }

    public virtual ProductPrice? CurrentPrice => Prices?.OrderByDescending(x => x.Date).FirstOrDefault();
    
    public virtual List<ProductImage> Images { get; set; }
    
    public virtual ProductImage? DefaultImage => Images?.Where(x => x.IsDefault).FirstOrDefault();
    
    protected Product()
    {
    }

    public Product(
        Guid id,
        Guid companyId,
        string name,
        ProductCategory productCategory,
        ProductType productType,
        string? shortDescription,
        string? fullDescription)
        : base(id)
    {
        CompanyId = companyId;
        SetName(name);
        SetCategory(productCategory);
        SetType(productType);
        ShortDescription = shortDescription;
        FullDescription = fullDescription;
        IsPublished = false;

        Prices = new List<ProductPrice>();
        Reviews = new List<ProductReview>();
        Images = new List<ProductImage>();
    }

    public Product SetName(string name)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        return this;
    }

    public Product SetCategory(ProductCategory category)
    {
        if(category == ProductCategory.Undefined)
        {
            throw new ArgumentException(WebMarketplaceDomainErrorCodes.ProductCategoryUndefinedNotAllowed);
        }

        ProductCategory = category;
        return this;
    }

    public Product SetType(ProductType type)
    {
        if (type == ProductType.Undefined)
        {
            throw new ArgumentException(WebMarketplaceDomainErrorCodes.ProductTypeUndefinedNotAllowed);
        }

        ProductType = type;
        return this;
    }

    public Product Publish(bool isPublish)
    {
        if (Prices.Any() && isPublish)
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
        if (Reviews.Any(x => x.UserId == userId))
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductReviewUserAlreadyExists);
        }

        var review = new ProductReview(reviewId, userId, this.Id, rating, comment);
        Reviews.Add(review);
        return this;
    }

    public Product UpdateReview(
        Guid reviewId,
        double rating,
        string? comment)
    {
        var review = Reviews.FirstOrDefault(x => x.Id == reviewId);
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
        var review = Reviews.FirstOrDefault(x => x.CreatorId == reviewId);
        if (review is null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductReviewNotFound);
        }

        Reviews.Remove(review);
        return this;
    }

    #endregion

    #region Prices

    public Product AddPrice(
        DateTime date,
        decimal amount,
        string currency)
    {
        if (Prices.Any(x => x.Date == date))
        {
            throw new ArgumentException(WebMarketplaceDomainErrorCodes.ProductReviewUserAlreadyExists);
        }

        var price = new ProductPrice(this.Id, date, amount, currency);
        Prices.Add(price);
        return this;
    }

    #endregion

    #region Images

    public Product AddImage(
        string blobName,
        bool isDefault)
    {
        var defaultImage = Images.FirstOrDefault(x => x.IsDefault);
        if (defaultImage is not null && isDefault)
        {
            defaultImage.IsDefault = false;
        }

        var image = new ProductImage(this.Id, blobName, isDefault);  
        Images.Add(image);
        return this;
    }
    
    public Product RemoveImage(string blobName)
    {
        var image = Images.FirstOrDefault(x => x.BlobName == blobName);
        if (image is null)
        {
            throw new ArgumentException(WebMarketplaceDomainErrorCodes.ProductImageNotFound);
        }
        else
        {
            if (image.IsDefault)
            {
                throw new ArgumentException(WebMarketplaceDomainErrorCodes.ProductImageDefaultRemoveNotAllowed);
            }
        }

        Images.Remove(image);
        return this;
    }

    #endregion
}