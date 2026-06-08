using E_Commerce.Application.DTOs.ProductDTOs;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result);
        }

        [HttpGet("{productId:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById([FromRoute] int productId)
        {
            var result = await _productService.GetProductByIdAsync(productId);
            if (result is null)
                return NotFound();

            return Ok(result);  
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProduct)
        {
            var result = await _productService.CreateProductAsync(createProduct);
            return CreatedAtAction(nameof(GetProductById), new { productId = result.Id }, result);
        }

        [HttpPut("{productId:int}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromBody] UpdateProductDto updateProduct)
        {
            await _productService.UpdateProductAsync(productId, updateProduct);
            return NoContent(); 
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
        {
            await _productService.DeleteProductAsync(productId);
            return NoContent();
        }
    }
}
