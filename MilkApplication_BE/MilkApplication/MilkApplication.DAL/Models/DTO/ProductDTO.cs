using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class ProductDTO
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int Price { get; set; }
        public string productDescription { get; set; }
        public string Image { get; set; }
        public int categoryId { get; set; }
        public int originId { get; set; }

    }
}
