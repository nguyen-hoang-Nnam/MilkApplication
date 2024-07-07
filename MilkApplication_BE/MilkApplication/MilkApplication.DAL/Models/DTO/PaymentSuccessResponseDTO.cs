using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class PaymentSuccessResponseDTO
    {
        public bool Success { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public List<ProductDTO> ProductDTOs { get; set; }
    }
}
