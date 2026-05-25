using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
    }
}
