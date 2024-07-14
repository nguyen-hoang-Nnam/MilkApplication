using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


    }
}
