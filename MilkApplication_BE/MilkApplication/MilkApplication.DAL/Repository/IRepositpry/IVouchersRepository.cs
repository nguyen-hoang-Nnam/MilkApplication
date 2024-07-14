using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IVouchersRepository : IGenericRepository<Vouchers>
    {
        Task<Vouchers> GetByCodeAsync(string code);
        Task<IEnumerable<Vouchers>> GetVouchersByStatusAsync(VouchersStatus status);
        void UpdateRange(IEnumerable<Vouchers> vouchers);
    }
}
