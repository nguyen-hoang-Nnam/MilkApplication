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
    public class VouchersRepository : GenericRepository<Vouchers>, IVouchersRepository
    {
        private readonly AppDbContext _context;
        public VouchersRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Vouchers> GetByCodeAsync(string code)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == code);
        }
        public async Task UpdateAsync(Vouchers voucher)
        {
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
        }
    }
}
