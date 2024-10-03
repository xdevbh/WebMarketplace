using WebMarketplace.Products;

namespace WebMarketplace.Orders;

public class OrderItemWithProduct
{
    public OrderItem OrderItem { get; set; }
    public Product Product { get; set; }

    public OrderItemWithProduct(OrderItem orderItem, Product product)
    {
        OrderItem = orderItem;
        Product = product;
    }
}