using MilkApplication.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task CreateOrderAsync(Order order, List<OrderItem> orderItems);
        Task DeleteOrderAsync(int orderId);
        Task<Order> GetOrderByIdWithItemAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
    }
}
