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
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllCategorysAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
        public Task<Pagination<Category>> GetCategoryByFilterAsync(PaginationParameter paginationParameter, CategoryFilterDTO categoryFilterDTO);
    }
}
