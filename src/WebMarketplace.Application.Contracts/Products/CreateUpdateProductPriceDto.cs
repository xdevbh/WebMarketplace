using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebMarketplace.Currencies;
using WebMarketplace.Localization;

namespace WebMarketplace.Products;

public class CreateUpdateProductPriceDto : IValidatableObject
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(3)]
    public string Currency { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var L = validationContext.GetRequiredService<IStringLocalizer<WebMarketplaceResource>>();

        if (Amount < 0)
        {
            yield return new ValidationResult(
                L[WebMarketplaceDomainErrorCodes.PriceNotNegative],
                new[] { nameof(Amount) }
            );
        }
    }
}
