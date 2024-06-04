using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Models;
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
    }
}
