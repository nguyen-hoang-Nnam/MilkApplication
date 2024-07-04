using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class CreatePaymentResult
    {
        public string PaymentUrl { get; set; }
        public string TransactionId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
