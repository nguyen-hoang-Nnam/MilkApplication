using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class Vouchers
    {
        [Key]
        public int voucherId { get; set; }
        public string Code { get; set; }
        public double? discountPercent { get; set; }
        public int quantity { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public VouchersStatus vouchersStatus { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
