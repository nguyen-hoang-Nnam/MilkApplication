using MilkApplication.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDTO> CreateOrderAsync(string userId, List<OrderItemDTO> orderItemDTO);
        Task<ResponseDTO> DeleteOrderAsync(int orderId);
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
    }
}
