using MilkApplication.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IComboService
    {
        public Task<List<ComboDTO>> GetAllCombosAsync();
        public Task<ComboDTO> GetComboByIdAsync(int id);
        public Task<ResponseDTO> AddComboAsync(ComboDTO comboDTO);
        public Task<ResponseDTO> UpdateComboAsync(int id, ComboDTO comboDTO);
        public Task<ResponseDTO> DeleteComboAsync(int id);
    }
}
