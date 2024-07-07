using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.PaginationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem);
        public Task<Pagination<OrderItem>> GetOrderItemByFilterAsync(PaginationParameter paginationParameter, OrderItemFilterDTO orderItemFilterDTO);
    }
}
