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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllCategorysAsync()
        {
            return await _context.Categories
                                 .Include(c => c.Products)
                                 .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }
        public async Task<Pagination<Category>> GetCategoryByFilterAsync(PaginationParameter paginationParameter, CategoryFilterDTO categoryFilterDTO)
        {
            try
            {
                var categoriesQuery = _context.Categories.AsQueryable();
                categoriesQuery = await ApplyFilterSortAndSearch(categoriesQuery, categoryFilterDTO);
                if (categoriesQuery != null)
                {
                    var categoryQuery = ApplySorting(categoriesQuery, categoryFilterDTO);
                    var totalCount = await categoryQuery.CountAsync();

                    var categoryPagination = await categoryQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<Category>(categoryPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<Category>> ApplyFilterSortAndSearch(IQueryable<Category> Query, CategoryFilterDTO categoryFilterDTO)
        {
            if (categoryFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(categoryFilterDTO.Search))
            {
                Query = Query.Where(x => x.categoryName.Contains(categoryFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<Category> ApplySorting(IQueryable<Category> query, CategoryFilterDTO categoryFilterDTO)
        {
            switch (categoryFilterDTO.Sort.ToLower())
            {
                case "categoryName":
                    query = (categoryFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.categoryName) : query.OrderBy(x => x.categoryName);
                    break;
                default:
                    query = (categoryFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.categoryId) : query.OrderBy(a => a.categoryId);
                    break;
            }
            return query;
        }
    }
}
