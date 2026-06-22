using Dapper;
using Marketplace.Orders.Application;
using Marketplace.Orders.Domain;
using Marketplace.Orders.Infrastructure.Helpers;

namespace Marketplace.Orders.Infrastructure.Implementation;

public class OrderRepository : IOrderRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public OrderRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(Order order)
    {
        using var connection = _connectionFactory.GetConnection();
        await connection.OpenAsync(); 
        await using var transaction = await connection.BeginTransactionAsync(); 

        var orderSql = @"
        INSERT INTO orders (id, userid, totalprice, status, createdat)
        VALUES (@Id, @UserId, @TotalPrice, @Status, @CreatedAt)";

        await connection.ExecuteAsync(orderSql, order, transaction);

        var itemSql = @"
        INSERT INTO order_items (id, orderid, productid, quantity, price)
        VALUES (@Id, @OrderId, @ProductId, @Quantity, @Price)";

        await connection.ExecuteAsync(itemSql, order.Items, transaction);

        await transaction.CommitAsync(); 
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.GetConnection();
        var order = await connection.QuerySingleOrDefaultAsync<Order>(
            "SELECT id, userid, totalprice, status, createdat FROM orders WHERE id = @Id", new { Id = id });

        if (order == null) return null;

        var items = await connection.QueryAsync<OrderItem>(
            "SELECT id, orderid, productid, quantity, price FROM order_items WHERE orderid = @OrderId",
            new { OrderId = id });

        order.Items = items.ToList();
        return order;
    }

    public async Task<IEnumerable<Order>> GetPagedAsync(Guid userId, int offset, int limit)
    {
        using var connection = _connectionFactory.GetConnection();

        var ordersSql = @"
        SELECT id, userid, totalprice, status, createdat 
        FROM orders 
        WHERE userid = @UserId 
        ORDER BY createdat DESC 
        LIMIT @Limit OFFSET @Offset";

        var orders = (await connection.QueryAsync<Order>(
            ordersSql, 
            new { UserId = userId, Limit = limit, Offset = offset }
        )).ToList();

        if (orders.Count == 0)
            return orders;

        var orderIds = orders.Select(o => o.Id).ToArray();
        var itemsSql = @"
        SELECT id, orderid, productid, quantity, price 
        FROM order_items 
        WHERE orderid = ANY(@OrderIds)";
    
        var items = await connection.QueryAsync<OrderItem>(
            itemsSql, 
            new { OrderIds = orderIds }
        );

        var itemsLookup = items.ToLookup(i => i.OrderId);
        foreach (var order in orders)
        {
            order.Items = itemsLookup[order.Id].ToList();
        }

        return orders;
    }

    public async Task UpdateStatusAsync(Guid id, OrderStatus status)
    {
        using var connection = _connectionFactory.GetConnection();
        var sql = "UPDATE orders SET status = @Status WHERE id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { Status = (int)status, Id = id });
        
        if (rowsAffected == 0)
            throw new KeyNotFoundException($"Заказ с Id {id} не найден.");
    }
}