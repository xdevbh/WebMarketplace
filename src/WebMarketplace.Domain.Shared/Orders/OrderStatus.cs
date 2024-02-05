namespace WebMarketplace.Orders;

public enum OrderStatus
{
    OrderCancelled,
    OrderDelivered,
    OrderInTransit,
    OrderPaymentDue,
    OrderPickupAvailable,
    OrderProblem,
    OrderProcessing,
    OrderReturned,
}