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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .Include(p => p.Origin)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.categoryId == categoryId)
                                 .ToListAsync();
        }
        /*public async Task<Product> GetByIdAsync(int? productId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId), "ProductId cannot be null.");
            }

            return await _context.Products.FindAsync(productId);
        }*/
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(c => c.productId == id);
        }
        public async Task<Pagination<Product>> GetProductByFilterAsync(PaginationParameter paginationParameter, ProductFilterDTO productFilterDTO)
        {
            try
            {
                var productsQuery = _context.Products.AsQueryable();
                productsQuery = await ApplyFilterSortAndSearch(productsQuery, productFilterDTO);
                if (productsQuery != null)
                {
                    var productQuery = ApplySorting(productsQuery, productFilterDTO);
                    var totalCount = await productQuery.CountAsync();

                    var productPagination = await productQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<Product>(productPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<Product>> ApplyFilterSortAndSearch(IQueryable<Product> Query, ProductFilterDTO productFilterDTO)
        {
            if (productFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(productFilterDTO.Search))
            {
                Query = Query.Where(x => x.productName.Contains(productFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<Product> ApplySorting(IQueryable<Product> query, ProductFilterDTO productFilterDTO)
        {
            switch (productFilterDTO.Sort.ToLower())
            {
                case "productName":
                    query = (productFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.productName) : query.OrderBy(x => x.productName);
                    break;
                default:
                    query = (productFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.productId) : query.OrderBy(a => a.productId);
                    break;
            }
            return query;
        }
    }
}
