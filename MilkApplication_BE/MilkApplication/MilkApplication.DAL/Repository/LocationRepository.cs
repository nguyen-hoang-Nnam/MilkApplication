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
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        private readonly AppDbContext _context;
        public LocationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations
                                 .Include(c => c.Products)
                                 .ToListAsync();
        }
        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            return await _context.Locations.FindAsync(locationId);
        }
        public async Task<Pagination<Location>> GetLocationByFilterAsync(PaginationParameter paginationParameter, LocationFilterDTO locationFilterDTO)
        {
            try
            {
                var locationsQuery = _context.Locations.AsQueryable();
                locationsQuery = await ApplyFilterSortAndSearch(locationsQuery, locationFilterDTO);
                if (locationsQuery != null)
                {
                    var locationQuery = ApplySorting(locationsQuery, locationFilterDTO);
                    var totalCount = await locationQuery.CountAsync();

                    var locationPagination = await locationQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<Location>(locationPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<Location>> ApplyFilterSortAndSearch(IQueryable<Location> Query, LocationFilterDTO locationFilterDTO)
        {
            if (locationFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(locationFilterDTO.Search))
            {
                Query = Query.Where(x => x.locationName.Contains(locationFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<Location> ApplySorting(IQueryable<Location> query, LocationFilterDTO locationFilterDTO)
        {
            switch (locationFilterDTO.Sort.ToLower())
            {
                case "locationName":
                    query = (locationFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.locationName) : query.OrderBy(x => x.locationName);
                    break;
                default:
                    query = (locationFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.locationId) : query.OrderBy(a => a.locationId);
                    break;
            }
            return query;
        }
    }
}
