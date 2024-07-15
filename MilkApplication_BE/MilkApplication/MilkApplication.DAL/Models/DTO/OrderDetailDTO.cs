using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class OrderDetailDTO
    {
        public int orderDetailId { get; set; }
        public int Quantity { get; set; }
        public int? productId { get; set; }
        /*public string productName { get; set; }*/
        public ProductDTO Product { get; set; }
    }
}
