using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Azure;
using MilkApplication.DAL.Repository.IRepositpry;

namespace MilkApplication.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService, UserManager<ApplicationUser> userManager, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        public async Task<ResponseDTO> CreateOrderAsync(CreateOrderDTO createOrderDto)
        {
            var response = new ResponseDTO();

            try
            {
                var user = await _unitOfWork.UserRepository.GetById(createOrderDto.Id);
                if (user == null)
                {
                    response.Message = "User not found.";
                    return response;
                }

                var orderItems = _mapper.Map<List<OrderDetail>>(createOrderDto.OrderDetails);
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
                if (createOrderDto.voucherId.HasValue && createOrderDto.voucherId.Value != 0)
                {
                    voucher = await _unitOfWork.VouchersRepository.GetByIdAsync(createOrderDto.voucherId.Value);
                    if (voucher == null || voucher.quantity < 1)
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
                    orderDate = DateTime.Now.AddHours(7),
                    Id = createOrderDto.Id,
                    UserName = user.UserName,
                    User = user,
                    OrderDetails = orderItems,
                    totalPrice = totalPrice,
                    voucherId = voucher?.voucherId,
                    Status = DAL.enums.OrderStatus.Unpaid

                };

                await _unitOfWork.OrderRepository.CreateOrderAsync(order, orderItems, createOrderDto.voucherId);

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

                var cancelUrl = "http://localhost:3000/";
                var returnUrl = "http://localhost:3000/";
                var paymentResult = await _paymentService.CreatePaymentLink(order, cancelUrl, returnUrl);

                if (!paymentResult.Success)
                {
                    response.Message = $"Failed to create payment link: {paymentResult.ErrorMessage}";
                    return response;
                }

                var orderDto = _mapper.Map<OrderDTO>(order);

                orderDto.PaymentUrl = paymentResult.PaymentUrl;

                response.IsSucceed = true;
                response.Message = "Order created successfully.";
                response.Data = orderDto;
            }
            catch (DbUpdateException dbEx)
            {
                // Log the inner exception for detailed error
                response.Message = $"Failed to create order: {dbEx.InnerException?.Message ?? dbEx.Message}";
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to create order: {ex.Message}";
            }

            return response;
        }
        public async Task<ResponseDTO> UpdateOrderAsync(int orderId, OrderStatus status, string staffId)
        {
            try
            {
                if (_unitOfWork == null)
                {
                    throw new NullReferenceException("_unitOfWork is null.");
                }

                var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = false,
                        Message = "Order not found."
                    };
                }

                var staff = await _unitOfWork.UserRepository.GetUserByIdAsync(staffId);
                if (staff == null || staff.Status != UserStatus.IsActive || !await _userManager.IsInRoleAsync(staff, UserRole.Staff.ToString()))
                {
                    return new ResponseDTO
                    {
                        IsSucceed = false,
                        Message = "Invalid staff ID, staff is not active, or staff does not have the required role."
                    };
                }

                order.Status = status;
                await _unitOfWork.OrderRepository.UpdateOrderAsync(order);
                await _unitOfWork.SaveChangeAsync();

                if (_mapper == null)
                {
                    throw new NullReferenceException("_mapper is null.");
                }

                var updatedOrderDTO = _mapper.Map<StaffUpdateOrderDTO>(order);
                updatedOrderDTO.StaffName = staff.FullName;

                var response = new ResponseDTO
                {
                    IsSucceed = true,
                    Message = $"{staff.FullName} updated order status successfully.",
                    Data = updatedOrderDTO
                };

                _orderRepository.SaveResponse(orderId, response);

                return response;
            }
            catch (NullReferenceException nullEx)
            {
                Console.WriteLine($"Null reference exception: {nullEx.Message}");
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "A null reference error occurred while updating the order."
                };
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "An error occurred while updating the order."
                };
            }
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

        public async Task<ResponseDTO> GetOrdersByCompletedPaymentsAsync()
        {
            var response = new ResponseDTO();

            try
            {
                // Fetch payments with completed status
                var completedPayments = await _unitOfWork.PaymentRepository.GetPaymentsByStatusAsync(PaymentStatus.PAID);

                if (!completedPayments.Any())
                {
                    response.Message = "No completed payments found.";
                    return response;
                }

                // Retrieve distinct order IDs from completed payments
                var orderIds = completedPayments.Select(p => p.orderId).Distinct();
                var orders = await _unitOfWork.OrderRepository.GetOrdersByIdsAsync(orderIds);

                // Map orders to DTOs
                var orderDtos = _mapper.Map<List<OrderDTO>>(orders);

                response.IsSucceed = true;
                response.Message = "Orders retrieved successfully.";
                response.Data = orderDtos;
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to retrieve orders: {ex.Message}";
            }

            return response;
        }

        public async Task<ResponseDTO> GetOrdersByCompletedPaymentsAsync(string userId)
        {
            var response = new ResponseDTO();

            try
            {
                // Fetch payments with completed status for the given userId
                var completedPayments = await _unitOfWork.PaymentRepository
                    .GetPaymentsByStatusAndUserIdAsync(PaymentStatus.PAID, userId);

                if (!completedPayments.Any())
                {
                    response.Message = "No completed payments found for the user.";
                    return response;
                }

                // Retrieve distinct order IDs from completed payments
                var orderIds = completedPayments.Select(p => p.orderId).Distinct();
                var orders = await _unitOfWork.OrderRepository.GetOrdersByIdsAsync(orderIds);

                // Map orders to DTOs
                var orderDtos = _mapper.Map<List<OrderDTO>>(orders);

                response.IsSucceed = true;
                response.Message = "Orders retrieved successfully.";
                response.Data = orderDtos;
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to retrieve orders: {ex.Message}";
            }

            return response;
        }

        public async Task<ResponseDTO> GetPaymentsByUserIdAsync(string userId)
        {
            var response = new ResponseDTO();

            try
            {
                // Fetch payments for the given userId
                var payments = await _unitOfWork.PaymentRepository.GetPaymentsByUserIdAsync(userId);

                if (!payments.Any())
                {
                    response.Message = "No payments found for the user.";
                    return response;
                }

                // Map payments to DTOs
                var paymentDtos = _mapper.Map<List<PaymentDTO>>(payments);

                response.IsSucceed = true;
                response.Message = "Payments retrieved successfully.";
                response.Data = paymentDtos;
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to retrieve payments: {ex.Message}";
            }

            return response;
        }

    }
}
