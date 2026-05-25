
using E_Commerce.Application.DTOs.OrderDTOs;

namespace E_Commerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> PlaceOrderAsync(PlaceOrderDto placeOrderDto, int orderId);
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId);

    }
}
