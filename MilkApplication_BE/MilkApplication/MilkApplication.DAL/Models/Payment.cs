using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.enums;

namespace MilkApplication.DAL.Models
{
    public class Payment
    {
        [Key]
        public int paymentId { get; set; }
        public int orderId { get; set; }
        [ForeignKey("orderId")]
        public Order Order { get; set; }
        public DateTime PaymentDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public int paymentMethodId { get; set; }
        [ForeignKey("paymentMethodId")]
        public PaymentMethod PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public string PaymentUrl { get; set; }
    }
}
