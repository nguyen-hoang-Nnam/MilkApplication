using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class OrderItemDTO
    {
        public int orderItemId { get; set; }
        public int Quantity { get; set; }
        public int? productId { get; set; }
    }
}
