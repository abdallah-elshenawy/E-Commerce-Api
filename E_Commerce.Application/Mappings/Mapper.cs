
using E_Commerce.Application.DTOs.CustomerDTOs;
using E_Commerce.Application.DTOs.OrderDTOs;
using E_Commerce.Application.DTOs.ProductDTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Mappings
{
    public static class Mapper
    {
        public static ProductDto MapProductToDto(Product product) => new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity
        };
        public static CustomerDto MapCustomerToDto(Customer customer) => new CustomerDto()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            CreatedAt = customer.CreatedAt
        };
        public static OrderItemDto MapOrderItemToDto(OrderItem orderItem) => new OrderItemDto()
        {
            ProductId = orderItem.ProductId,
            ProductName = orderItem.Product?.Name ?? string.Empty,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice
        };
        public static OrderDto MapOrderToDto(Order order) => new OrderDto()
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            TotalPrice = order.CalculateTotalPrice(),
            OrderItems = order.OrderItems.Select(oi => Mapper.MapOrderItemToDto(oi)).ToList()
        };
    }
}
