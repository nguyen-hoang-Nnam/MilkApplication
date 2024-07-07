using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class VouchersDTO
    {
        public int voucherId { get; set; }
        public string Code { get; set; }
        public int discountPercent { get; set; }
        public int quantity { get; set; }
        public DateTime date { get; set; }
        public VouchersStatus vouchersStatus { get; set; }
    }
}
