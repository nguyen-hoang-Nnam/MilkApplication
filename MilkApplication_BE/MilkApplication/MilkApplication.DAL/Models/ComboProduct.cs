using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class ComboProduct
    {
        [Key]
        public int comboProductId { get; set; }
        public int comboId { get; set; }
        [ForeignKey("comboId")]
        public Combo? Combo { get; set; }
        public int productId { get; set; }
        [ForeignKey("productId")]
        public Product? Product { get; set; }
        public ComboStatus Status { get; set; }
    }
}
