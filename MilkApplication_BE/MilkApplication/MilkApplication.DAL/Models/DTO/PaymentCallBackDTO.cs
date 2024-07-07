using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class PaymentCallBackDTO 
    {
        public int orderId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public UserDTO userDTO { get; set; }
    }
}
