using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.DTOs;

public record UpdateOrderStatusDto(OrderStatus Status);