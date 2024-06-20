using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class OrderDTO
    {
        public int orderId { get; set; }
        public DateTime orderDate { get; set; }
        public decimal totalPrice { get; set; }
        public string Id { get; set; }
    }
}
