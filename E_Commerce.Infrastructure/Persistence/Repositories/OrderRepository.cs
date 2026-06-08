using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders.AsNoTracking().Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .AsNoTracking().SingleOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
