using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace WebMarketplace.Products;

public class ProductPrice : Entity
{
    public virtual Guid ProductId { get; set; }
    public virtual DateTime Date { get; set; }
    public virtual decimal Amount { get; private set; }
    public virtual string Currency { get; private set; }
    
    protected ProductPrice()
    {
    }

    public ProductPrice(Guid productId, DateTime date, decimal amount, string currency)
    {
        ProductId = productId;
        Date = date;
        SetAmount(amount);
        SetCurrency(currency);
    }

    public ProductPrice SetAmount(decimal amount)
    {
        if (amount < 0)
            throw new BusinessException(WebMarketplaceDomainErrorCodes.PriceNotNegative);
        
        Amount = amount;
        return this;
    }
    
    
    public ProductPrice SetCurrency(string currency)
    {
        Check.NotNullOrWhiteSpace(currency, nameof(currency));
        Check.Length(currency, nameof(currency), WebMarketplaceConsts.CurrencyCodeLength, WebMarketplaceConsts.CurrencyCodeLength);

        Currency = currency.ToUpper();

        return this;
    }

    public override object?[] GetKeys()
    {
        return new object?[] { ProductId, Date };
    }
}