

using E_Commerce.Application.DTOs.CustomerDTOs;

namespace E_Commerce.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto?> GetCustomerByIdAsync(int customerId);
    }
}