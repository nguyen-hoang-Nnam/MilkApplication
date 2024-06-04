using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IOriginService
    {
        public Task<List<OriginDTO>> GetAllOriginsAsync();
        public Task<OriginDTO> GetOriginByIdAsync(int id);
        public Task<ResponseDTO> AddOriginAsync(OriginDTO originDTO);
        public Task<ResponseDTO> UpdateOriginAsync(int id, OriginDTO originDTO);
        public Task<ResponseDTO> DeleteOriginAsync(int id);
    }
}
