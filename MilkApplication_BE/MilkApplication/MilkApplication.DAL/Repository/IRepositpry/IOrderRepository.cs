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

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task CreateOrderAsync(Order order, List<OrderDetail> orderItems, int? voucherId);
        Task DeleteOrderAsync(int orderId);
        Task<Order> GetOrderByIdWithItemAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        public Task<Pagination<Order>> GetOrderByFilterAsync(PaginationParameter paginationParameter, OrderFilterDTO orderFilterDTO);
        Task UpdateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByIdsAsync(IEnumerable<int> orderIds);
        Task<List<Order>> GetOrderByIdsAsync(IEnumerable<int> orderIds);
        void SaveResponse(int orderId, ResponseDTO response);
        ResponseDTO GetResponse(int orderId);
    }
}
