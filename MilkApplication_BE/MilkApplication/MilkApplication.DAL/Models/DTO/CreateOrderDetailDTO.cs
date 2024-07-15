using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class CreateOrderDetailDTO
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
