using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
    }
}
