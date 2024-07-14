using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.enums;
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

        public async Task<IEnumerable<Vouchers>> GetVouchersByStatusAsync(VouchersStatus status)
        {
            return await _context.Vouchers
                .Where(v => v.vouchersStatus == status)
                .ToListAsync();
        }

        public void UpdateRange(IEnumerable<Vouchers> vouchers)
        {
            _context.Vouchers.UpdateRange(vouchers);
        }
    }
}
