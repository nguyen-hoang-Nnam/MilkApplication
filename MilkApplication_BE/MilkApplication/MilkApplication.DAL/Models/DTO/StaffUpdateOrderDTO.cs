using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class StaffUpdateOrderDTO
    {
        public string UserName { get; set; }
        public int orderId { get; set; }
        public DateTime orderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal totalPrice { get; set; }
        public int voucherId { get; set; }
        public string Id { get; set; }
        public string? PaymentUrl { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public VouchersDTO Voucher { get; set; }
        public string StaffName { get; set; }
    }
}
