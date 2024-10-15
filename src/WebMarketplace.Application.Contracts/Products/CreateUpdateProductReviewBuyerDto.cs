using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using WebMarketplace.Localization;

namespace WebMarketplace.Products;

public class CreateUpdateProductReviewBuyerDto : IValidatableObject
{
    [Required]
    public virtual Guid ProductId { get; set; }
    
    [Required]
    public virtual int Rating { get;  set; }
    
    public virtual string? Comment { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var L = validationContext.GetRequiredService<IStringLocalizer<WebMarketplaceResource>>();

        if (Rating < ProductConsts.RatingMinValue || Rating > ProductConsts.RatingMaxValue)
        {
            yield return new ValidationResult(
                L[WebMarketplaceDomainErrorCodes.ProductReviewRantingRange],
                new[] { nameof(Rating) }
            );
        }
    }
}