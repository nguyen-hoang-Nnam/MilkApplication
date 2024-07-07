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
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(int locationId);
        public Task<Pagination<Location>> GetLocationByFilterAsync(PaginationParameter paginationParameter, LocationFilterDTO locationFilterDTO);
    }
}
