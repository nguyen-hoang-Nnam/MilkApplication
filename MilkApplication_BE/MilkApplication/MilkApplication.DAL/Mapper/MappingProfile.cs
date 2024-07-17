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
            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.productName, opt => opt.MapFrom(src => src.Product.productName))
                .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<CommentDTO, Comment>()
                .ForMember(dest => dest.Product, opt => opt.Ignore()) // Product will be set separately
                .ForMember(dest => dest.User, opt => opt.Ignore());
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
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses)); // Map addresses
            CreateMap<ApplicationUser, StaffDTO>();
            CreateMap<StaffDTO, ApplicationUser>();
            CreateMap<ApplicationUser, AdminDTO>();
            CreateMap<AdminDTO, ApplicationUser>();
            CreateMap<Vouchers, VouchersDTO>().ReverseMap();
            CreateMap<VouchersDTO, Vouchers>()
                .ForMember(dest => dest.voucherId, opt => opt.Ignore());
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, StaffUpdateOrderDTO>().ReverseMap();
            CreateMap<Order, StaffUpdateOrderDTO>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Voucher, opt => opt.MapFrom(src => new VouchersDTO
            {
                voucherId = src.Voucher.voucherId,
                Code = src.Voucher.Code,
                discountPercent = src.Voucher.discountPercent,
                quantity = src.Voucher.quantity,
                dateFrom = src.Voucher.dateFrom,
                dateTo = src.Voucher.dateTo,
                vouchersStatus = src.Voucher.vouchersStatus
            }))
            .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>src.User.Email))
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Voucher, opt => opt.MapFrom(src => new VouchersDTO
            {
                voucherId = src.Voucher.voucherId,
                Code = src.Voucher.Code,
                discountPercent = src.Voucher.discountPercent,
                quantity = src.Voucher.quantity,
                dateFrom = src.Voucher.dateFrom,
                dateTo = src.Voucher.dateTo,
                vouchersStatus = src.Voucher.vouchersStatus
            }));
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<Order, OrderRequestDTO>().ReverseMap();
            CreateMap<OrderRequestDTO, Order>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetail));
            CreateMap<CreateOrderDTO, Order>();
            CreateMap<CreateOrderDetailDTO, OrderDetail>();
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

            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<Payment, PaymentDTO>()
                /*.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Order.User.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Order.User.FullName))*/
                /*.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Order.User.Email))*/
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, Address>();
        }
    }

}
