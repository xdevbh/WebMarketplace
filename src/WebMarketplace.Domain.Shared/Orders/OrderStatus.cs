namespace WebMarketplace.Orders;

public enum OrderStatus
{
    Cancelled = 0,
    New = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Completed = 5,
    Refunded = 6,
    Failed = 7
}