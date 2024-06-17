using MilkApplication.DAL.Models;
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
    }
}
