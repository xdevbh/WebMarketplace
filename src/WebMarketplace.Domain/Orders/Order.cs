using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using WebMarketplace.Currencies;

namespace WebMarketplace.Orders;

public class Order : FullAuditedAggregateRoot<Guid>
{
    public virtual Buyer Buyer { get; private set; }
    public virtual Guid CompanyId { get; set; }
    public virtual string CompanyName { get; private set; }
    public virtual OrderStatus Status { get; private set; }
    public virtual decimal TotalPrice { get; private set; }
    public virtual string Currency { get; private set; }
    public virtual List<OrderItem> Items { get; private set; }
    
    public virtual ShippingAddress ShippingAddress { get; private set; }
    public int ItemsCount => Items.Count;
    public int TotalQuantity => Items.Sum(x => x.Quantity);

    protected Order()
    {
    }

    public Order(
        Guid id,
        Buyer buyer,
        Guid companyId, 
        string companyName, 
        ShippingAddress shippingAddress,
        decimal? totalPrice = null, 
        string? currency = null)
    : base(id)
    {
        Buyer = buyer;
        CompanyId = companyId;
        SetCompanyName(companyName);
        Status = OrderStatus.New;
        Items = new();
        SetTotalPrice(totalPrice);
        SetCurrency(currency);
        ShippingAddress = shippingAddress;
    }

    public Order SetCompanyName(string companyName)
    {
        Check.NotNull(companyName, nameof(companyName));

        CompanyName = companyName;
        return this;
    }

    public Order SetCurrency(string? currency)
    {
        Check.NotNull(currency, nameof(currency));
        Check.Length(currency, nameof(currency), WebMarketplaceConsts.CurrencyCodeLength,
            WebMarketplaceConsts.CurrencyCodeLength);

        if(Currency.IsNullOrEmpty()) 
        {
            Currency = currency;
        }
        else
        {
            if (Currency != currency) // items currency should be the same
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.CurrencyAlreadySet);
            }
        }

        return this;
    }

    public Order SetTotalPrice(decimal? totalPrice = null)
    {
        var price = totalPrice ?? Items.Sum(item => item.TotalPrice);
        if (price < 0)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.PriceNotNegative);
        }

        TotalPrice = price;
        return this;
    }

    #region OrderItems

    public Order AddItem(
        Guid id,
        Guid productId,
        string productName,
        int quantity,
        decimal unitPrice,
        string currency,
        decimal? totalPrice = null)
    {
        SetCurrency(currency);
        var existingItem = Items.Find(x => x.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.SetQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            Items.Add(new OrderItem(
                id,
                this.Id,
                productId,
                productName,
                quantity,
                unitPrice,
                currency,
                totalPrice
            ));
        }

        return this;
    }

    public Order RemoveItem(Guid productId)
    {
        var existingItem = Items.Find(x => x.ProductId == productId);
        if (existingItem != null)
        {
            Items.Remove(existingItem);
        }

        return this;
    }

    public Order ChangeStatus(OrderStatus status)
    {
        Status = status;
        return this;
    }

    public Order ChangeQuantity(Guid productId, int quantity)
    {
        var existingItem = Items.Find(x => x.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.SetQuantity(quantity);
        }

        return this;
    }

    #endregion
}