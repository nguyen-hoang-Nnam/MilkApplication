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
    public interface IOriginRepository : IGenericRepository<Origin>
    {
        Task<IEnumerable<Origin>> GetAllOriginsAsync();
        Task<Origin> GetOriginByIdAsync(int originId);
        public Task<Pagination<Origin>> GetOriginByFilterAsync(PaginationParameter paginationParameter, OriginFilterDTO originFilterDTO);
    }
}
