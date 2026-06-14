namespace Marketplace.Orders.Domain;

public enum OrderStatus
{
    Created = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}