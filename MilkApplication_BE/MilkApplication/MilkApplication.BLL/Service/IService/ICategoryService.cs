using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models.PaginationDTO;

namespace MilkApplication.BLL.Service.IService
{
    public interface ICategoryService
    {
        public Task<List<CategoryDTO>> GetAllCategorysAsync();
        public Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<List<CategoryDetailDTO>> GetAllCategoriesWithProductsAsync();
        public Task<ResponseDTO> AddCategoryAsync(CategoryDTO categoryDTO);
        public Task<ResponseDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDTO);
        public Task<ResponseDTO> DeleteCategoryAsync(int id);
        public Task<Pagination<CategoryDTO>> GetCategoryByFilterAsync(PaginationParameter paginationParameter, CategoryFilterDTO categoryFilterDTO);
    }
}
