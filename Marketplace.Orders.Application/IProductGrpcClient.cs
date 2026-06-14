namespace Marketplace.Orders.Application;

public interface IProductGrpcClient
{
    Task<ProductInfo?> GetProductByIdAsync(Guid productId);
}

public record ProductInfo(Guid Id, decimal Price);