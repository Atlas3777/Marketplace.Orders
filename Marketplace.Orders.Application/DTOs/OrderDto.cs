using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid UserId,
    List<OrderItemDto> Items,
    decimal TotalPrice,
    OrderStatus Status,
    DateTime CreatedAt);