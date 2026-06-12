
using AutoMapper;
using E_Commerce.Application.DTOs.CustomerDTOs;
using E_Commerce.Application.DTOs.OrderDTOs;
using E_Commerce.Application.DTOs.ProductDTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>();

            CreateMap<Customer, CustomerDto>();

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.CalculateTotalPrice()));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
        }
    }
}
