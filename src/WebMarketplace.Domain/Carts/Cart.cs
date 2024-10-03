using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace WebMarketplace.Carts;

public class Cart : AggregateRoot<Guid>
{
    public List<CartItem> Items { get; set; } = new();

    protected Cart()
    {
    }

    public Cart(Guid id)
        : base(id)
    {
    }

    public Cart AddItem(Guid productId, int quantity = 1)
    {
        if (quantity < 1)
        {
            throw new ArgumentException("Product quantity should be 1 or more!");
        }

        var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.AddQuantity(quantity);
        }
        else
        {
            Items.Add(new CartItem(productId, quantity));
        }

        return this;
    }

    public Cart RemoveItem(Guid productId, int? quantity = null)
    {
        if (quantity is < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Product quantity should be null, 1 or more!");
        }

        var item = Items.FirstOrDefault(x => x.ProductId == productId);
        if (item == null)
        {
            return this;
        }

        if (quantity == null || item.Quantity <= quantity)
        {
            Items.Remove(item);
            return this;
        }

        item.AddQuantity(-quantity.Value);
        return this;
    }

    public int GetProductQuantity(Guid productId)
    {
        var item = Items.FirstOrDefault(x => x.ProductId == productId);
        return item?.Quantity ?? 0;
    }

    public void Clear()
    {
        Items.Clear();
    }

    public void Merge(Cart cart)
    {
        foreach (var item in cart.Items)
        {
            AddItem(item.ProductId, item.Quantity);
        }
    }
}