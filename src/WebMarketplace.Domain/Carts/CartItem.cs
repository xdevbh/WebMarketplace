using System;
using Volo.Abp;

namespace WebMarketplace.Carts;

public class CartItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; private set; }

    protected CartItem()
    {
        
    }
    
    public CartItem(Guid productId, int quantity)
    {
        ProductId = productId; 
        SetQuantity(quantity);
    }
    
    public CartItem SetQuantity(int quantity)
    {
        Check.Positive(quantity, nameof(quantity));

        Quantity = quantity;
        return this;
    }
    
    public CartItem AddQuantity(int quantity)
    {
        Check.Positive(Quantity + quantity, nameof(quantity));

        Quantity += quantity;
        return this;
    }
}