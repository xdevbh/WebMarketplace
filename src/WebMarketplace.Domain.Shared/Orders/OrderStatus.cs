namespace WebMarketplace.Orders;

public enum OrderStatus
{
    Cancelled = 0,
    New = 1,
    PendingPayment = 2,
    Paid = 3,
    Processing = 4,
    Shipped = 5,
    Delivered = 6,
    Completed = 7,
    Refunded = 8,
    Failed = 9
}