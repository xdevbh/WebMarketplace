using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebMarketplace.Currencies;
using WebMarketplace.Localization;

namespace WebMarketplace.Products;

public class CreateUpdateProductDto : IValidatableObject
{
    public Guid? CompanyId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public ProductCategory ProductCategory { get; set; }

    [Required]
    public ProductType ProductType { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var L = validationContext.GetRequiredService<IStringLocalizer<WebMarketplaceResource>>();

        if (ProductCategory == ProductCategory.Undefined)
        {
            yield return new ValidationResult(
                L[WebMarketplaceDomainErrorCodes.ProductCategoryUndefinedNotAllowed],
                new[] { nameof(ProductCategory) }
            );
        }

        if (ProductType == ProductType.Undefined)
        {
            yield return new ValidationResult(
                L[WebMarketplaceDomainErrorCodes.ProductTypeUndefinedNotAllowed],
                new[] { nameof(ProductType) }
            );
        }
    }
}