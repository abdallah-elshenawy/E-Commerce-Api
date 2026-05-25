using E_Commerce.Application.DTOs.CustomerDTOs;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int customerId)
        {
            int authenticatedCustomerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer is null || authenticatedCustomerId != customerId)
                return NotFound();

            return Ok(customer);
        }
    }
}
