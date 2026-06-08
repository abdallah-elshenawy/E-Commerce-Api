
namespace E_Commerce.Application.DTOs.OrderDTOs
{
    public class PlaceOrderDto
    {
        public IEnumerable<OrderItemRequestDto> Items { get; set; } = new List<OrderItemRequestDto>();
    }
}
