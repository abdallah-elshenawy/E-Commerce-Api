
using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.ProductDTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Entities;
using FluentAssertions;
using Moq;

namespace E_Commerce.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _productService;
        public ProductServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cacheMock = new Mock<ICacheService>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_unitOfWorkMock.Object, _cacheMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetProductById_CacheHit_ReturnsValidProductDto()
        {
            
            ProductDto productDto = new ProductDto()
            {
                Id = 1, Description = "desc", Name = "product", Price = 100m, StockQuantity = 20
            };
            _cacheMock.Setup(c => c.GetAsync<ProductDto>(CacheKeys.Product(productDto.Id))).ReturnsAsync(productDto);
            
            ProductDto? actual = await _productService.GetProductByIdAsync(productDto.Id);

            
            _unitOfWorkMock.Verify(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>()), Times.Never);
            actual.Should().Be(productDto);
        }

        [Fact]
        public async Task GetProductById_CacheMissAndProductDoesntExist_ReturnsNull()
        { 
            _cacheMock.Setup(c => c.GetAsync<ProductDto>(It.IsAny<string>())).ReturnsAsync((ProductDto?)null);
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

            
            ProductDto? actual = await _productService.GetProductByIdAsync(10);

            
            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetProductById_CacheMissAndProductExist_ReturnsProductDto()
        {
            Product product = new Product("product", "desc", 100m, 20, 1);
            _cacheMock.Setup(c => c.GetAsync<ProductDto>(It.IsAny<string>())).ReturnsAsync((ProductDto?)null);
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto());
            
            ProductDto? actual = await _productService.GetProductByIdAsync(10);

            
            actual.Should().NotBeNull();
        }


        [Fact]
        public async Task GetProductById_CacheMiss_SetCache()
        {
            
            
            Product product = new Product("product", "desc", 100m, 20, 1);
            _cacheMock.Setup(c => c.GetAsync<ProductDto>(It.IsAny<string>())).ReturnsAsync((ProductDto?)null);
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto());

            ProductDto? actual = await _productService.GetProductByIdAsync(10);

            
            _cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProductDto>(), null), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_ValidateTheProductProperties_CreateProductWithValidProps()
        {
            _unitOfWorkMock.Setup(u => u.ProductRepository.Add(It.IsAny<Product>()));
            CreateProductDto createProductDto = new CreateProductDto()
            {
                Description = "desc", Name = "product", Price = 100m, StockQuantity = 20
            };
            Product product = new Product("product", "desc", 100m, 20, 1);
            ProductDto productDto = new ProductDto()
            {
                Description = "desc", Name = "product", Price = 100m, StockQuantity = 20
            };
            _mapperMock.Setup(m => m.Map<ProductDto>(It.IsAny<Product>())).Returns(productDto);


            var actual = await _productService.CreateProductAsync(createProductDto);

            
            actual.Name.Should().Be(createProductDto.Name);
            actual.Description.Should().Be(createProductDto.Description);
            actual.Price.Should().Be(createProductDto.Price);
            actual.StockQuantity.Should().Be(createProductDto.StockQuantity);
        }

        [Fact]
        public async Task CreateProductAsync_VerifyingAddAndSaveAndRemoveCache_AddAndSaveAndCacheShouldBeCalledOnce()
        {
            
            
            _unitOfWorkMock.Setup(u => u.ProductRepository.Add(It.IsAny<Product>()));
            CreateProductDto createProductDto = new CreateProductDto()
            {
                Description = "desc",
                Name = "product",
                Price = 100m,
                StockQuantity = 20
            };

            
            var actual = await _productService.CreateProductAsync(createProductDto);

            
            _unitOfWorkMock.Verify(u => u.ProductRepository.Add(It.IsAny<Product>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ProductIsNull_ThrowsNotFoundException()
        {
            
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(async () => await _productService.UpdateProductAsync(99, new UpdateProductDto()));
        }

        [Fact]
        public async Task UpdateProductAsync_ProductIsFound_UpdateAndSaveAreCalledOnceAndRemoveCacheCalledTwice()
        {
            
            Product product = new Product("product", "desc", 100m, 20, 1);
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);
            _unitOfWorkMock.Setup(u => u.ProductRepository.Update(It.IsAny<Product>()));

            
            UpdateProductDto productDto = new UpdateProductDto()
            {
                Description = "desc",
                Name = "product",
                Price = 100m
            };
            await _productService.UpdateProductAsync(10, productDto);

            
            _unitOfWorkMock.Verify(u => u.ProductRepository.Update(It.IsAny<Product>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DeleteProductAsync_ProductIsNull_ThrowsNotFoundException()
        {
            
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(async () => await _productService.DeleteProductAsync(10));
        }

        [Fact]
        public async Task DeleteProductAsync_ProductIsFound_DeleteAndSaveCalledOnceAndRemoveCacheCalledTwice()
        {
            
            
            Product product = new Product("product", "desc", 100m, 20, 1);
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);
            _unitOfWorkMock.Setup(u => u.ProductRepository.Delete(It.IsAny<Product>()));

            
            await _productService.DeleteProductAsync(10);

            
            _unitOfWorkMock.Verify(u => u.ProductRepository.Delete(It.IsAny<Product>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
