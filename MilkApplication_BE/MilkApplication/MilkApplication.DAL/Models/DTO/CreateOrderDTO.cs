using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class CreateOrderDTO
    {
        public string Id { get; set; }
        public List<CreateOrderDetailDTO> OrderDetails { get; set; }
        public int? voucherId { get; set; }
    }
}
