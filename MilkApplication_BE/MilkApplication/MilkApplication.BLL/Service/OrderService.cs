using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> CreateOrderAsync(string userId, List<OrderItemDTO> orderItemsDto, string? voucherCode = null)
        {
            var response = new ResponseDTO();

            try
            {
                var user = await _unitOfWork.UserRepository.GetById(userId);
                if (user == null)
                {
                    response.Message = "User not found.";
                    return response;
                }

                var orderItems = _mapper.Map<List<OrderItem>>(orderItemsDto);
                foreach (var item in orderItems)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.productId.Value);
                    if (product == null)
                    {
                        response.Message = $"Product with ID {item.productId} not found.";
                        return response;
                    }

                    if (product.Quantity < item.Quantity)
                    {
                        response.Message = $"Insufficient stock for product with ID {item.productId}.";
                        return response;
                    }

                    if (product.discountPrice.HasValue)
                    {
                        item.Price = product.discountPrice.Value;
                    }
                    else if (product.discountPercent.HasValue && product.discountPercent.Value > 0)
                    {
                        item.Price = product.Price * (1 - (decimal)(product.discountPercent.Value / 100));
                    }
                    else
                    {
                        item.Price = product.Price;
                    }
                }

                Vouchers voucher = null;
                if (!string.IsNullOrEmpty(voucherCode))
                {
                    voucher = await _unitOfWork.VouchersRepository.GetByCodeAsync(voucherCode);
                    if (voucher == null || voucher.vouchersStatus == VouchersStatus.Expired || voucher.quantity < 1)
                    {
                        response.Message = "Invalid or expired voucher.";
                        return response;
                    }

                    foreach (var item in orderItems)
                    {
                        item.Price = item.Price * (1 - (decimal)(voucher.discountPercent / 100));
                    }

                    voucher.quantity -= 1;
                    voucher.vouchersStatus = (voucher.quantity > 0) ? voucher.vouchersStatus : VouchersStatus.Expired;
                }

                decimal totalPrice = orderItems.Sum(oi => oi.Quantity * oi.Price);

                var order = new Order
                {
                    orderDate = DateTime.UtcNow,
                    Id = userId,
                    UserName = user.UserName,
                    User = user,
                    OrderItems = orderItems,
                    totalPrice = totalPrice,
                    voucherId = voucher.voucherId
                };

                await _unitOfWork.OrderRepository.CreateOrderAsync(order, orderItems);

                foreach (var item in orderItems)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.productId.Value);
                    if (product != null)
                    {
                        product.Quantity -= item.Quantity;
                        await _unitOfWork.ProductRepository.UpdateAsync(product);
                    }
                }

                if (voucher != null)
                {
                    await _unitOfWork.VouchersRepository.UpdateAsync(voucher);
                }

                await _unitOfWork.SaveChangeAsync();

                var orderDto = _mapper.Map<OrderDTO>(order);

                response.IsSucceed = true;
                response.Message = "Order created successfully.";
                response.Data = orderDto;
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to create order: {ex.Message}";
            }

            return response;
        }

        public async Task<ResponseDTO> DeleteOrderAsync(int orderId)
        {
            var response = new ResponseDTO();

            try
            {
                await _unitOfWork.OrderRepository.DeleteOrderAsync(orderId);
                await _unitOfWork.SaveChangeAsync();

                response.IsSucceed = true;
                response.Message = "Order deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to delete order: {ex.Message}";
            }

            return response;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<Order> GetOrderEntityByIdAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(orderId);
            return order;
        }

        public async Task<Pagination<OrderDTO>> GetOrderByFilterAsync(PaginationParameter paginationParameter, OrderFilterDTO orderFilterDTO)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetOrderByFilterAsync(paginationParameter, orderFilterDTO);
                if (orders != null)
                {
                    var mapperResult = _mapper.Map<List<OrderDTO>>(orders);
                    return new Pagination<OrderDTO>(mapperResult, orders.TotalCount, orders.CurrentPage, orders.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
