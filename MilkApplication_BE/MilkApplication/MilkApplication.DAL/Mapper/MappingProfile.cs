﻿using AutoMapper;
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
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Origin, OriginDTO>().ReverseMap();
            CreateMap<OriginDTO, Origin>()
            .ForMember(dest => dest.originId, opt => opt.Ignore());
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
            CreateMap<Combo, ComboDTO>().ReverseMap();
            CreateMap<ComboProduct, ComboProductDTO>().ReverseMap();

            CreateMap<ComboProduct, ComboProductDTO>()
            .ForMember(dest => dest.ComboName, opt => opt.MapFrom(src => src.Combo.comboName))
            .ForMember(dest => dest.ComboPrice, opt => opt.MapFrom(src => src.Combo.comboPrice))
            .ForMember(dest => dest.ComboDiscountPrice, opt => opt.MapFrom(src => src.Combo.discountPrice))
            .ForMember(dest => dest.ComboDiscountPercent, opt => opt.MapFrom(src => src.Combo.discountPercent))
            .ForMember(dest => dest.ComboDescription, opt => opt.MapFrom(src => src.Combo.comboDescription))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.productName));

            CreateMap<ComboProductCreateDTO, ComboProduct>().ReverseMap();
            CreateMap<ComboProduct, ComboProductCreateDTO>()
            .ForMember(dest => dest.ComboName, opt => opt.MapFrom(src => src.Combo.comboName))
            .ForMember(dest => dest.ComboPrice, opt => opt.MapFrom(src => src.Combo.comboPrice))
            .ForMember(dest => dest.ComboDiscountPrice, opt => opt.MapFrom(src => src.Combo.discountPrice))
            .ForMember(dest => dest.ComboDiscountPercent, opt => opt.MapFrom(src => src.Combo.discountPercent))
            .ForMember(dest => dest.ComboDescription, opt => opt.MapFrom(src => src.Combo.comboDescription))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.productName));
        }
    }
}
