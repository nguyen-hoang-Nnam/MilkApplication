using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDTO> CreateOrderAsync(string userId, List<OrderDetailDTO> orderItemDTO, int? voucherId);
        Task<ResponseDTO> DeleteOrderAsync(int orderId);
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<Order> GetOrderEntityByIdAsync(int orderId);
        public Task<Pagination<OrderDTO>> GetOrderByFilterAsync(PaginationParameter paginationParameter, OrderFilterDTO orderFilterDTO);

        Task<ResponseDTO> GetOrdersByCompletedPaymentsAsync();
    }
}
