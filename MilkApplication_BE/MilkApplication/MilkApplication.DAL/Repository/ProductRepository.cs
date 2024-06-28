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
                                 .Include(p => p.Category)
                                 .Include(p => p.Origin)
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
    }
}
