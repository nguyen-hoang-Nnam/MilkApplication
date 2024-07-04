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
    public interface ILocationService
    {
        public Task<List<LocationDTO>> GetAllLocationsAsync();
        public Task<LocationDTO> GetLocationByIdAsync(int id);
        public Task<ResponseDTO> AddLocationAsync(LocationDTO locationDTO);
        public Task<ResponseDTO> UpdateLocationAsync(int id, LocationDTO locationDTO);
        public Task<ResponseDTO> DeleteLocationAsync(int id);
        public Task<Pagination<LocationDTO>> GetLocationByFilterAsync(PaginationParameter paginationParameter, LocationFilterDTO locationFilterDTO);
    }
}
