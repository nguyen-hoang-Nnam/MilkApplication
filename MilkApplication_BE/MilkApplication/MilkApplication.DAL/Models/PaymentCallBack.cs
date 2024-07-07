using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class PaymentCallBack
    {
        [Key]
        public int PaymentCallbackId { get; set; }

        public int orderId { get; set; }
        [ForeignKey("orderId")]
        public Order Order { get; set; }

        public string TransactionId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public string Status { get; set; } 
        public string PaymentMethod { get; set; }
        public DateTime CallbackDate { get; set; }
    }
}
