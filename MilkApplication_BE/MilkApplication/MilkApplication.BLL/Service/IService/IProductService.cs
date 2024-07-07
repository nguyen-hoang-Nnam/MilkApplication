using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IProductService
    {
        public Task<List<ProductDTO>> GetAllProductsAsync();
        public Task<ProductDetailDTO> GetProductByIdAsync(int id);
        public Task<ResponseDTO> AddProductAsync(ProductDTO productDTO);
        public Task<ResponseDTO> UpdateProductAsync(int id, ProductDTO productDTO);
        public Task<ResponseDTO> DeleteProductAsync(int id);
        public Task<List<ProductDTO>> GetProductsByCategoryIdAsync(int categoryId);
        public Task<Pagination<ProductDTO>> GetProductByFilterAsync(PaginationParameter paginationParameter, ProductFilterDTO productFilterDTO);
    }
}
