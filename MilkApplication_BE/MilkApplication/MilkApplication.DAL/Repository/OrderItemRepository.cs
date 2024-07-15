using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class OrderItemRepository : GenericRepository<OrderDetail>, IOrderItemRepository
    {
        private readonly AppDbContext _context;
        public OrderItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OrderDetail> CreateOrderItemAsync(OrderDetail orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }
        public async Task<Pagination<OrderDetail>> GetOrderItemByFilterAsync(PaginationParameter paginationParameter, OrderItemFilterDTO orderItemFilterDTO)
        {
            try
            {
                var orderItemsQuery = _context.OrderItems.AsQueryable();
                orderItemsQuery = await ApplyFilterSortAndSearch(orderItemsQuery, orderItemFilterDTO);
                if (orderItemsQuery != null)
                {
                    var orderItemQuery = ApplySorting(orderItemsQuery, orderItemFilterDTO);
                    var totalCount = await orderItemQuery.CountAsync();

                    var orderItemPagination = await orderItemQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<OrderDetail>(orderItemPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<OrderDetail>> ApplyFilterSortAndSearch(IQueryable<OrderDetail> Query, OrderItemFilterDTO orderItemFilterDTO)
        {
            if (orderItemFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(orderItemFilterDTO.Search))
            {
                Query = Query.Where(x => x.Order.orderId.Equals(orderItemFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<OrderDetail> ApplySorting(IQueryable<OrderDetail> query, OrderItemFilterDTO orderItemFilterDTO)
        {
            switch (orderItemFilterDTO.Sort.ToLower())
            {
                case "price":
                    query = (orderItemFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price);
                    break;
                default:
                    query = (orderItemFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.orderId) : query.OrderBy(a => a.orderId);
                    break;
            }
            return query;
        }
    }
}
