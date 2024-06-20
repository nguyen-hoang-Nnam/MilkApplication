using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
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

        public async Task<ResponseDTO> CreateOrderAsync(string userId, List<OrderItemDTO> orderItemsDto)
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
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.productId);
                    if (product == null)
                    {
                        response.Message = $"Product with ID {item.productId} not found.";
                        return response;
                    }
                    item.Price = product.Price; // Set Price from the product
                }

                decimal totalPrice = orderItems.Sum(oi => oi.Quantity * oi.Price);

                var order = new Order
                {
                    orderDate = DateTime.UtcNow,
                    Id = userId,
                    User = user,
                    OrderItems = orderItems, // Assign order items
                    totalPrice = totalPrice
                };

                await _unitOfWork.OrderRepository.CreateOrderAsync(order, orderItems);
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
    }
}
