using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;

namespace MilkApplication.BLL.Service.IService
{
    public interface IAddressService
    {
        public Task<ResponseDTO> AddAddressAsync(AddressDTO addressDTO);
        public Task<List<AddressDTO>> GetAllAddressAsync();
        public Task<ResponseDTO> UpdateAddressAsync(int id, AddressDTO addressDTO);
        public Task<ResponseDTO> DeleteAddressAsync(int id);
    }
}
