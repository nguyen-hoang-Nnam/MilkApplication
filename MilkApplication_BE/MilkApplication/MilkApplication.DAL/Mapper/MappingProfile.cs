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
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<Product, ProductDetailDTO>().ReverseMap();
            CreateMap<Product, ProductDetailCategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryDetailDTO>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Origin, OriginDTO>().ReverseMap();
            CreateMap<OriginDTO, Origin>()
            .ForMember(dest => dest.originId, opt => opt.Ignore());
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Comment, CommentDetailDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => new List<CommentUserDetailDTO>
            {
                new CommentUserDetailDTO
                {
                    FullName = src.User.FullName,
                    Id = src.User.Id
                }
            }))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
            CreateMap<Comment, CommentUserDetailDTO>().ReverseMap();    
            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<Vouchers, VouchersDTO>().ReverseMap();
            CreateMap<VouchersDTO, Vouchers>()
            .ForMember(dest => dest.voucherId, opt => opt.Ignore());
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDeatils));
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>()
            .ForMember(dest => dest.productName, opt => opt.MapFrom(src => src.Product.productName));
            CreateMap<Order, OrderRequestDTO>().ReverseMap();
            CreateMap<OrderRequestDTO, Order>()
                .ForMember(dest => dest.OrderDeatils, opt => opt.MapFrom(src => src.OrderDetailDTOs));
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
