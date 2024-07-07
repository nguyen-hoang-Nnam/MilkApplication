using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class PaymentStatusResult
    {
        public bool Success { get; set; }
        public PaymentStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
