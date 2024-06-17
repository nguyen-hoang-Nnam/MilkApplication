using MilkApplication.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IVouchersService
    {
        public Task<List<VouchersDTO>> GetAllVouchersAsync();
        public Task<VouchersDTO> GetVouchersByIdAsync(int id);
        public Task<ResponseDTO> AddVouchersAsync(VouchersDTO originDTO);
        public Task<ResponseDTO> UpdateVouchersAsync(int id, VouchersDTO originDTO);
        public Task<ResponseDTO> DeleteVouchersAsync(int id);
    }
}
