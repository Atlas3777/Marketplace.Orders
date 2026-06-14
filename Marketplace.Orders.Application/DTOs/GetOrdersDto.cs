namespace Marketplace.Orders.Application.DTOs;

public record GetOrdersDto(int PageIndex = 0, int PageSize = 10);