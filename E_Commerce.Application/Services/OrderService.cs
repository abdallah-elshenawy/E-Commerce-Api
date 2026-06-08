

using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.OrderDTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderDto> PlaceOrderAsync(PlaceOrderDto placeOrderDto, int customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (customer is null)
                throw new NotFoundException("Customer", customerId);

            var productIds = placeOrderDto.Items.Select(p => p.ProductId).ToList();
            var products = await _unitOfWork.ProductRepository.GetProductsByIdsAsync(productIds);

            Dictionary<int, Product> productMap = products.ToDictionary(p => p.Id);

            Order order = new Order(customerId);
            foreach (var item in placeOrderDto.Items)
            {
                if (!productMap.TryGetValue(item.ProductId, out var product))
                    throw new NotFoundException("Product", item.ProductId);

                order.AddItem(new OrderItem(product.Id, item.Quantity, product.Price));
                product.ReduceStock(item.Quantity);
            }

            _unitOfWork.OrderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync();
            OrderDto orderDto = _mapper.Map<OrderDto>(order);
            return orderDto;
        }
        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            Order? order = await _unitOfWork.OrderRepository.GetOrderWithItemsAsync(orderId);
            if (order is null)
                return null;

            OrderDto orderDto = _mapper.Map<OrderDto>(order);
            return orderDto;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByCustomerIdAsync(customerId);

            List<OrderDto> orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return orderDtos;
        }

    }
}
