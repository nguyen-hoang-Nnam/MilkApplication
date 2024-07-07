using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class PaymentConfirmationResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public PaymentDTO Payment { get; set; }
    }
}
