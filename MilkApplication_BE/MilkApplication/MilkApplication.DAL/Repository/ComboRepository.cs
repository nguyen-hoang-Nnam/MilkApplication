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
    public class ComboRepository : GenericRepository<Combo>, IComboRepository
    {
        private readonly AppDbContext _context;
        public ComboRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Combos.AnyAsync(c => c.comboId == id);
        }
    }
}
