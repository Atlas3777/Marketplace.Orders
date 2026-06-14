using Marketplace.Orders.Application.DTOs;
using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Api.Mappers;

public static class OrderMappingExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            Id: order.Id,
            UserId: order.UserId,
            TotalPrice: order.TotalPrice,
            Status: order.Status,
            CreatedAt: order.CreatedAt,
            Items: order.Items.Select(i => i.ToDto()).ToList()
        );
    }

    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto(
            Id: item.Id,
            ProductId: item.ProductId,
            Quantity: item.Quantity,
            Price: item.Price
        );
    }
}