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
    public class ComboProductRepository : GenericRepository<ComboProduct>, IComboProductRepository
    {
        private readonly AppDbContext _context;
        public ComboProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComboProduct>> GetAllComboProductAsync()
        {
            return await _context.ComboProducts
                .Include(cp => cp.Combo)
                .Include(cp => cp.Product)
                .ToListAsync();
        }

        public async Task<ComboProduct> GetComboProductByIdAsync(int id)
        {
            return await _context.ComboProducts
                .Include(cp => cp.Combo)
                .Include(cp => cp.Product)
                .FirstOrDefaultAsync(cp => cp.comboProductId == id);
        }
        public async Task<Pagination<ComboProduct>> GetComboProductByFilterAsync(PaginationParameter paginationParameter, ComboProductFilterDTO comboProductFilterDTO)
        {
            try
            {
                var productsQuery = _context.ComboProducts.AsQueryable();
                productsQuery = await ApplyFilterSortAndSearch(productsQuery, comboProductFilterDTO);
                if (productsQuery != null)
                {
                    var productQuery = ApplySorting(productsQuery, comboProductFilterDTO);
                    var totalCount = await productQuery.CountAsync();

                    var productPagination = await productQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<ComboProduct>(productPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<ComboProduct>> ApplyFilterSortAndSearch(IQueryable<ComboProduct> Query, ComboProductFilterDTO comboProductFilterDTO)
        {
            if (comboProductFilterDTO == null)
            {
                return Query;
            }
            if (comboProductFilterDTO.productId != null)
            {
                Query = Query.Where(less => less.productId == comboProductFilterDTO.productId);
            }
            return Query;
        }
        private IQueryable<ComboProduct> ApplySorting(IQueryable<ComboProduct> query, ComboProductFilterDTO comboProductFilterDTO)
        {
            switch (comboProductFilterDTO.Sort.ToLower())
            {
                case "comboId":
                    query = (comboProductFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.Combo.comboName) : query.OrderBy(x => x.Combo.comboName);
                    break;
                default:
                    query = (comboProductFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.productId) : query.OrderBy(a => a.productId);
                    break;
            }
            return query;
        }
    }
}
