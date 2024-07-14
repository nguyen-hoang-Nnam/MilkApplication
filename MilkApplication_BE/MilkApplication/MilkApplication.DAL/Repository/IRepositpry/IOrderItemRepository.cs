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
    public interface IOrderItemRepository : IGenericRepository<OrderDetails>
    {
        Task<OrderDetails> CreateOrderItemAsync(OrderDetails orderItem);
        public Task<Pagination<OrderDetails>> GetOrderItemByFilterAsync(PaginationParameter paginationParameter, OrderItemFilterDTO orderItemFilterDTO);
    }
}
