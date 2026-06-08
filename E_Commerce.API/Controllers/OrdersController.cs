using E_Commerce.Application.DTOs.OrderDTOs;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        public async Task<ActionResult<OrderDto>> PlaceOrder([FromBody] PlaceOrderDto placeOrderDto)
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            OrderDto orderDto = await _orderService.PlaceOrderAsync(placeOrderDto, customerId);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = orderDto.Id }, orderDto);
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById([FromRoute] int orderId)
        {
            int customerId = int.Parse((User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
            OrderDto orderDto = await _orderService.GetOrderByIdAsync(orderId);
            if (orderDto is null || customerId != orderDto.CustomerId)
                return NotFound();

            return Ok(orderDto);
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            IEnumerable<OrderDto> orderDtos = await _orderService.GetOrdersByCustomerIdAsync(customerId);
            return Ok(orderDtos);
        }
    }
}
