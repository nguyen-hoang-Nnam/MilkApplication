using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment> GetByTransactionIdAsync(string transactionId);
        Task UpdatePaymentAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<List<Payment>> GetPendingPaymentsAsync();
        Task<decimal> SumAsync(Expression<Func<Payment, bool>> predicate, Expression<Func<Payment, decimal>> selector);
        Task<Dictionary<string, decimal>> GetMonthlyTotalsAsync(int months);
        Task<List<Payment>> GetPaymentsByStatusAndUserIdAsync(PaymentStatus status, string userId);
        Task<List<Payment>> GetPaymentsByUserIdAsync(string userId);

    }
}
