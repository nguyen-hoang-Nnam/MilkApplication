using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class OrderRequestDTO
    {
        public string Id { get; set; }
        public List<OrderDetailDTO> OrderDetail { get; set; }
        public int? voucherId { get; set; }
    }
}
