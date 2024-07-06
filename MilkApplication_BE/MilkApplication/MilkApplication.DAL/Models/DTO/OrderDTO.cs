using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.enums;

namespace MilkApplication.DAL.Models.DTO
{
    public class OrderDTO
    {
        public string UserName { get; set; }
        public int orderId { get; set; }
        public DateTime orderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal totalPrice { get; set; }
        public string Id { get; set; }
    }
}
