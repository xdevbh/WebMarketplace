using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Orders;

public class OrderItem : AuditedEntity<Guid>
{
    public virtual Guid OrderId { get; set; }
    public virtual Guid ProductId { get; set; }
    public virtual string ProductName { get; private set; }
    public virtual int Quantity { get; private set; }
    public virtual decimal UnitPrice { get; private set; }
    public virtual string Currency { get; private set; }
    public virtual decimal TotalPrice { get; private set; }

    protected OrderItem()
    {
    }

    public OrderItem(Guid id,
        Guid orderId,
        Guid productId,
        string productName,
        int quantity,
        decimal unitPrice,
        string currency,
        decimal? totalPrice = null)
        : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        SetProductName(productName);
        SetQuantity(quantity);
        SetUnitPrice(unitPrice);
        SetCurrency(currency); 
        SetTotalPrice(totalPrice);
    }
    
    public OrderItem SetProductName(string productName)
    {
        Check.NotNull(productName, nameof(productName));

        ProductName = productName;
        return this;
    }

    public OrderItem SetQuantity(int quantity)
    {
        Check.Positive(quantity, nameof(quantity));

        Quantity = quantity;
        return this;
    }

    public OrderItem SetUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.PriceNotNegative);
        }

        UnitPrice = unitPrice;
        return this;
    }

    public OrderItem SetTotalPrice(decimal? totalPrice)
    {
        var price = totalPrice ?? Quantity * UnitPrice;
        if (price < 0)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.PriceNotNegative);
        }

        TotalPrice = price;
        return this;
    }
    
    public OrderItem SetCurrency(string currency)
    {
        Check.NotNullOrWhiteSpace(currency, nameof(currency));
        Check.Length(currency, nameof(currency), WebMarketplaceConsts.CurrencyCodeLength, WebMarketplaceConsts.CurrencyCodeLength);

        Currency = currency;

        return this;
    }
    
    
}