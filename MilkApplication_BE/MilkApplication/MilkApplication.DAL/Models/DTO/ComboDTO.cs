using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class ComboDTO
    {
        public int comboId { get; set; }
        public string comboName { get; set; }
        public decimal comboPrice { get; set; }
        public double discountPercent { get; set; }
        public decimal discountPrice { get; set; }
        public string comboDescription { get; set; }
    }
}
