using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("Invalid transaction ID.", nameof(transactionId));
            return await _context.Payments.SingleOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _context.Payments
            .Where(p => p.Status == status)
            .Include(p => p.Order)
                .ThenInclude(o => o.User)
            .Include(p => p.Order)
                .ThenInclude(o => o.OrderDetails)
                    .ThenInclude(oi => oi.Product)
            .ToListAsync();
        }

        public async Task<List<Payment>> GetPendingPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == PaymentStatus.Pending)
                .ToListAsync();
        }

        public async Task<decimal> SumAsync(Expression<Func<Payment, bool>> predicate, Expression<Func<Payment, decimal>> selector)
        {
            return await _context.Payments.Where(predicate).SumAsync(selector);
        }

        public async Task<Dictionary<string, decimal>> GetMonthlyTotalsAsync(int months)
        {
            var result = new Dictionary<string, decimal>();
            var currentDate = DateTime.Now;

            for (int i = 0; i < months; i++)
            {
                var date = currentDate.AddMonths(-i);
                var totalAmount = await _context.Payments
                    .Where(p => p.PaymentDate.Year == date.Year && p.PaymentDate.Month == date.Month)
                    .SumAsync(p => p.Amount);
                result.Add(date.ToString("yyyy-MM"), totalAmount);
            }

            return result;
        }

        public async Task<List<Payment>> GetPaymentsByStatusAndUserIdAsync(PaymentStatus status, string userId)
        {
            return await _context.Payments
                .Where(p => p.Status == status && p.Order.User.Id == userId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByUserIdAsync(string userId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.User)
                 .Include(p => p.Order)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                 .Include(p => p.Order)
                        .ThenInclude(o => o.Voucher)
                .Where(p => p.Order.User.Id == userId)
                .ToListAsync();
        }
    }
}
