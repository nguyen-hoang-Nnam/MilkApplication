using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<T> GetById(string id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<Pagination<T>> ToPagination(PaginationParameter paginationParameter)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                                    .Take(paginationParameter.Limit)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<T>(items, itemCount, paginationParameter.Page, paginationParameter.Limit);

            return result;
        }

    }
}
