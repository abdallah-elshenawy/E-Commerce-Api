using E_Commerce.Application.Interfaces;
using E_Commerce.Infrastructure.Persistence.Repositories;

namespace E_Commerce.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        private ICustomerRepository? _customerRepository;
        private IOrderRepository? _orderRepository;
        private IProductRepository? _productRepository;
        private IRefreshTokenRepository? _refreshTokenRepository;

        public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_context);
        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_context);
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);
        public IRefreshTokenRepository RefreshTokenRepository => 
            _refreshTokenRepository ??= new RefreshTokenRepository(_context);
 

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
