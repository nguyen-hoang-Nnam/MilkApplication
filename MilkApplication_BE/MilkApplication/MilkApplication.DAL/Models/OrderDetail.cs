using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class OrderDetail
    {
        [Key]
        public int orderDetailId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int? productId { get; set; }
        [ForeignKey("productId")]
        public Product? Product { get; set; }
        public int? orderId { get; set; }
        [ForeignKey("orderId")]
        public Order? Order { get; set; }
    }
}
