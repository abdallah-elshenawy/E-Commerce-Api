

using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRepository ProductRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        Task<int> SaveChangesAsync(); 
    }
}
