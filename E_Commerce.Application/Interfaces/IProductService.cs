using E_Commerce.Application.DTOs.ProductDTOs;

namespace E_Commerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(int productId, UpdateProductDto updateProductDto);
        Task DeleteProductAsync(int productId);
    }
}
