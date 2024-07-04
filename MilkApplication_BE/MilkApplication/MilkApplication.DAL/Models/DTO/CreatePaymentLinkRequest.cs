using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class CreatePaymentLinkRequest
    {
        public int orderId { get; set; }
        public string CancelUrl { get; set; }
        public string ReturnUrl { get; set; }
    }
}
