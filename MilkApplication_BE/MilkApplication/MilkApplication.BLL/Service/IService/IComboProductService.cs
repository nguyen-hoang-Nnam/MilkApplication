using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IComboProductService
    {
        public Task<List<ComboProductDTO>> GetAllComboProductsAsync();
        public Task<ComboProductCreateDTO> GetComboProductByIdAsync(int id);
        public Task<ResponseDTO> AddComboProductAsync(ComboProductCreateDTO comboProductCreateDTO);
        public Task<ResponseDTO> UpdateComboProductAsync(int id, ComboProductDTO comboProductDTO);
        public Task<ResponseDTO> DeleteComboProductAsync(int id);
        public Task<Pagination<ComboProductDTO>> GetComboProductByFilterAsync(PaginationParameter paginationParameter, ComboProductFilterDTO comboProductFilterDTO);
    }
}
