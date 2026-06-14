namespace Marketplace.Orders.Domain;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Created; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}