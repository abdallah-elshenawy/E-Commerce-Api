using AutoMapper;
using E_Commerce.Application.DTOs.CustomerDTOs;
using E_Commerce.Application.Interfaces;

namespace E_Commerce.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (customer is null)
                return null;

            CustomerDto customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }
    }
}
