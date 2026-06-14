namespace Marketplace.Orders.Application.DTOs;

public record OrderItemDto(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal Price);