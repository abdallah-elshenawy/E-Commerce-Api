using E_Commerce.Application.DTOs.CustomerDTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Mappings;

namespace E_Commerce.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (customer is null)
                return null;

            CustomerDto customerDto = Mapper.MapCustomerToDto(customer);
            return customerDto;
        }
    }
}
