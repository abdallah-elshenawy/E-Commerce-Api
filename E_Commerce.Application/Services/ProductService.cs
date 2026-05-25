using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.ProductDTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Mappings;
using E_Commerce.Domain.Entities;
namespace E_Commerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;

        public ProductService(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            List<ProductDto>? cachedRes = await _cache.GetAsync<List<ProductDto>>(CacheKeys.AllProducts);
            if (cachedRes is not null)
                return cachedRes;
            
            IEnumerable<Product> products = await _unitOfWork.ProductRepository.GetAllAsync();
            var productDtos = products.Select(Mapper.MapProductToDto).ToList();
            await _cache.SetAsync(CacheKeys.AllProducts, productDtos);
            return productDtos;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            ProductDto? cachedRes = await _cache.GetAsync<ProductDto>(CacheKeys.Product(productId));
            if (cachedRes is not null)
                return cachedRes;

            Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product is null)
                return null;

            ProductDto productDto = Mapper.MapProductToDto(product);
            await _cache.SetAsync(CacheKeys.Product(productId), productDto);
            return productDto;
        }
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            Product product = new Product(createProductDto.Name, createProductDto.Description, 
                createProductDto.Price, createProductDto.StockQuantity);

            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();

            ProductDto productDto = Mapper.MapProductToDto(product);
            await _cache.RemoveAsync(CacheKeys.AllProducts);
            return productDto;
        }

        public async Task UpdateProductAsync(int productId, UpdateProductDto updateProductDto)
        {

            Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product is null)
                throw new NotFoundException("Product", productId);

            product.UpdateDetails(updateProductDto.Name, updateProductDto.Price, updateProductDto.Description);
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            await _cache.RemoveAsync(CacheKeys.Product(productId));
            await _cache.RemoveAsync(CacheKeys.AllProducts);
        }
        public async Task DeleteProductAsync(int productId)
        {
            Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product is null)
                throw new NotFoundException("Product", productId);

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            await _cache.RemoveAsync(CacheKeys.Product(productId));
            await _cache.RemoveAsync(CacheKeys.AllProducts);
        }

    }
}
