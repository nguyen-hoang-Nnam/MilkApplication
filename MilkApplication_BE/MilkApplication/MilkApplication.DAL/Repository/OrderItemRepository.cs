using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly AppDbContext _context;
        public OrderItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }
    }
}
