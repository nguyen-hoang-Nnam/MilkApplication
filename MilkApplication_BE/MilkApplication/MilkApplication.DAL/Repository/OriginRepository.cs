using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class OriginRepository : GenericRepository<Origin>, IOriginRepository
    {
        private readonly AppDbContext _context;
        public OriginRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Origin>> GetAllOriginsAsync()
        {
            return await _context.Origins
                                 .Include(c => c.Products)
                                 .ToListAsync();
        }
        public async Task<Origin> GetOriginByIdAsync(int originId)
        {
            return await _context.Origins.FindAsync(originId);
        }
        public async Task<Pagination<Origin>> GetOriginByFilterAsync(PaginationParameter paginationParameter, OriginFilterDTO originFilterDTO)
        {
            try
            {
                var originsQuery = _context.Origins.AsQueryable();
                originsQuery = await ApplyFilterSortAndSearch(originsQuery, originFilterDTO);
                if (originsQuery != null)
                {
                    var originQuery = ApplySorting(originsQuery, originFilterDTO);
                    var totalCount = await originQuery.CountAsync();

                    var originPagination = await originQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<Origin>(originPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<Origin>> ApplyFilterSortAndSearch(IQueryable<Origin> Query, OriginFilterDTO originFilterDTO)
        {
            if (originFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(originFilterDTO.Search))
            {
                Query = Query.Where(x => x.originName.Contains(originFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<Origin> ApplySorting(IQueryable<Origin> query, OriginFilterDTO originFilterDTO)
        {
            switch (originFilterDTO.Sort.ToLower())
            {
                case "locationName":
                    query = (originFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.originName) : query.OrderBy(x => x.originName);
                    break;
                default:
                    query = (originFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.originId) : query.OrderBy(a => a.originId);
                    break;
            }
            return query;
        }
    }
}
