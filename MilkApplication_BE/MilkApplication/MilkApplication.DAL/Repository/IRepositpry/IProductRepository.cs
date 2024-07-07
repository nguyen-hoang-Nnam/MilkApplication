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
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        /*Task<Product> GetByIdAsync(int? productId);*/
        Task<bool> ExistsAsync(int id);
        public Task<Pagination<Product>> GetProductByFilterAsync(PaginationParameter paginationParameter, ProductFilterDTO productFilterDTO);
    }
}
