using AutoMapper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Origin, OriginDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<Vouchers, VouchersDTO>().ReverseMap();
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Order, OrderRequestDTO>().ReverseMap();
            CreateMap<OrderRequestDTO, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemDTOs));
        }
    }
}
