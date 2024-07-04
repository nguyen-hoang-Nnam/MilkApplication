using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order order, List<OrderItem> orderItems)
        {
            order.OrderItems = orderItems;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Delete associated OrderItems
            var orderItems = _context.OrderItems.Where(oi => oi.orderId == orderId);
            _context.OrderItems.RemoveRange(orderItems);

            // Now delete the Order
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByIdWithItemAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.orderId == orderId);
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                                 .Include(o => o.OrderItems) // Include related entities as needed
                                 .FirstOrDefaultAsync(o => o.orderId == orderId);
        }
    }
}
