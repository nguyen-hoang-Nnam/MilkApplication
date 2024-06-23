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
    }
}
