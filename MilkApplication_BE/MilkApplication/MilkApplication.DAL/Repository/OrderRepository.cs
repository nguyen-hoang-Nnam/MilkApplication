using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.PaginationDTO;
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
        public async Task<Pagination<Order>>GetOrderByFilterAsync(PaginationParameter paginationParameter, OrderFilterDTO orderFilterDTO)
        {
            try
            {
                var ordersQuery = _context.Orders.AsQueryable();
                ordersQuery = await ApplyFilterSortAndSearch(ordersQuery, orderFilterDTO);
                if (ordersQuery != null)
                {
                    var orderQuery = ApplySorting(ordersQuery, orderFilterDTO);
                    var totalCount = await orderQuery.CountAsync();

                    var orderPagination = await orderQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<Order>(orderPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<Order>> ApplyFilterSortAndSearch(IQueryable<Order> Query, OrderFilterDTO orderFilterDTO)
        {
            if (orderFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(orderFilterDTO.Search))
            {
                Query = Query.Where(x => x.Id.Contains(orderFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<Order> ApplySorting(IQueryable<Order> query, OrderFilterDTO orderFilterDTO)
        {
            switch (orderFilterDTO.Sort.ToLower())
            {
                case "Id":
                    query = (orderFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                    break;
                default:
                    query = (orderFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.orderId) : query.OrderBy(a => a.orderId);
                    break;
            }
            return query;
        }
    }
}
