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
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.Product) // Assuming Product is the navigation property
                .Include(c => c.User) // Assuming User is the navigation property
                .ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByProductIdAsync(int productId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.productId == productId)
                .ToListAsync();
        }
        public async Task<Pagination<Comment>> GetCommentByFilterAsync(PaginationParameter paginationParameter, CommentFilterDTO commentFilterDTO)
        {
            try
            {
                var commentsQuery = _context.Comments.AsQueryable();
                commentsQuery = await ApplyFilterSortAndSearch(commentsQuery, commentFilterDTO);
                if (commentsQuery != null)
                {
                    var commentQuery = ApplySorting(commentsQuery, commentFilterDTO);
                    var totalCount = await commentQuery.CountAsync();

                    var commentPagination = await commentQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<Comment>(commentPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<Comment>> ApplyFilterSortAndSearch(IQueryable<Comment> Query, CommentFilterDTO commentFilterDTO)
        {
            if (commentFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(commentFilterDTO.Search))
            {
                Query = Query.Where(x => x.Id.Contains(commentFilterDTO.Search));
            }
            if (commentFilterDTO.productId != null)
            {
                Query = Query.Where(less => less.productId == commentFilterDTO.productId);
            }
            return Query;
        }
        private IQueryable<Comment> ApplySorting(IQueryable<Comment> query, CommentFilterDTO commentFilterDTO)
        {
            switch (commentFilterDTO.Sort.ToLower())
            {
                case "commentDetail":
                    query = (commentFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.commentDetail) : query.OrderBy(x => x.commentDetail);
                    break;
                default:
                    query = (commentFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }
            return query;
        }
    }
}
