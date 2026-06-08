
using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.OrderDTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Exceptions;
using FluentAssertions;
using Moq;
using System;

namespace E_Commerce.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrderService _orderService;
        public OrderServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _orderService = new OrderService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task PlaceOrderAsync_TheCustomerNotFound_ThrowsNotFoundException()
        {
            
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Customer?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(async () => await _orderService.PlaceOrderAsync(new PlaceOrderDto(), 10));
        }

        [Fact]
        public async Task PlaceOrderAsync_TheProductNotFound_ThrowsNotFoundException()
        {
            
            Customer customer = new Customer("fname", "sname", "email");
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((customer));

            List<Product> products = new List<Product>
            {
                new Product("product1", "desc", 100m, 20) { Id = 1 },
                new Product("product2", "desc", 100m, 20) { Id = 2 },
            };
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetProductsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(products);

            PlaceOrderDto placeOrderDto = new PlaceOrderDto
            {
                Items = new List<OrderItemRequestDto>
                {
                    new OrderItemRequestDto
                    {
                        ProductId = 5,
                        Quantity = 8
                    },
                    new OrderItemRequestDto
                    {
                        ProductId = 6,
                        Quantity = 10
                    },
                }
            };

            
            await Assert.ThrowsAsync<NotFoundException>(async () => await _orderService.PlaceOrderAsync(placeOrderDto, 1));
        }

        [Fact]
        public async Task PlaceOrderAsync_TheQuantityIsInsufficient_ThrowsInsufficientStockException()
        {
            
            Customer customer = new Customer("fname", "sname", "email");
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((customer));
            List<Product> products = new List<Product>
            {
                new Product("product1", "desc", 100m, 20) { Id = 1 },
                new Product("product2", "desc", 100m, 20) { Id = 2 },
            };
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetProductsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(products);
            PlaceOrderDto placeOrderDto = new PlaceOrderDto
            {
                Items = new List<OrderItemRequestDto>
                {
                    new OrderItemRequestDto
                    {
                        ProductId = 1,
                        Quantity = 25
                    },
                    new OrderItemRequestDto
                    {
                        ProductId = 6,
                        Quantity = 10
                    },
                }
            };

            
            await Assert.ThrowsAsync<InsufficientStockException>(async () => await _orderService.PlaceOrderAsync(placeOrderDto, 1));
        }

        [Fact]
        public async Task PlaceOrderAsync_CreatingOrder_OrderCreatedAndStockDecreasedAndSaveChangesOnce()
        {
            
            Customer customer = new Customer("fname", "sname", "email");
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(customer);
            List<Product> products = new List<Product>
            {
                new Product("product1", "desc", 100m, 20) { Id = 1 },
                new Product("product2", "desc", 100m, 20) { Id = 2 },
            };
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetProductsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(products);
            _unitOfWorkMock.Setup(u => u.OrderRepository.Add(It.IsAny<Order>()));
            PlaceOrderDto placeOrderDto = new PlaceOrderDto()
            {
                Items = new List<OrderItemRequestDto>()
                {
                    new OrderItemRequestDto() {ProductId = 1, Quantity = 10},
                    new OrderItemRequestDto() {ProductId = 2, Quantity = 8}
                }
            };

            await _orderService.PlaceOrderAsync(placeOrderDto, customer.Id);
            
            _unitOfWorkMock.Verify(u => u.OrderRepository.
            Add(It.Is<Order>(o => 
                o.OrderItems.Count == 2 &&
                o.OrderItems.Any(i => i.ProductId == 1 && i.Quantity == 10) &&
                o.OrderItems.Any(i => i.ProductId == 2 && i.Quantity == 8))), Times.Once);
            products[0].StockQuantity.Should().Be(10);
            products[1].StockQuantity.Should().Be(12);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

        }

        [Fact]
        public async Task GetOrderByIdAsync_OrderIsNotFound_ReturnsNull()
        {
            _unitOfWorkMock.Setup(u => u.OrderRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order?)null);

            var actual = await _orderService.GetOrderByIdAsync(5);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetOrderByIdAsync_OrderExist_ReturnsValidOrderDto()
        {
            Order order = new Order(1);
            _unitOfWorkMock.Setup(u => u.OrderRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(order);
            _mapperMock.Setup(m => m.Map<OrderDto>(It.IsAny<Order>())).Returns(new OrderDto());

            var actual = await _orderService.GetOrderByIdAsync(5);

            actual.Should().NotBeNull();

        }
    }
}
