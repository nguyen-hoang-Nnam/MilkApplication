using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string PaymentUrl { get; set; }
        public string orderId { get; set; }
        public string Id { get; set; }
    }
}
