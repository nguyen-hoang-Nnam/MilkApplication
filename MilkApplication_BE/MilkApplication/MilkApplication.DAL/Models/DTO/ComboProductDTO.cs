using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class ComboProductDTO
    {
        public int comboId { get; set; }
        public string ComboName { get; set; }
        public decimal ComboPrice { get; set; }
        public decimal? ComboDiscountPrice { get; set; }
        public double? ComboDiscountPercent { get; set; }
        public string ComboDescription { get; set; }
        public int productId { get; set; }
        public string ProductName { get; set; }
    }
}
