using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class Combo
    {
        [Key]
        public int comboId { get; set; }
        public string comboName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal comboPrice { get; set; }
        private decimal? _discountPrice;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? discountPrice
        {
            get => _discountPrice;
            private set => _discountPrice = value;
        }
        private double? _discountPercent;
        public double? discountPercent
        {
            get => _discountPercent;
            set
            {
                _discountPercent = value;
                if (_discountPercent.HasValue && _discountPercent > 0)
                {
                    discountPrice = comboPrice * (1 - (decimal)(_discountPercent.Value / 100));
                }
                else
                {
                    discountPrice = null;
                }
            }
        }
        public string comboDescription { get; set; }
        public ICollection<ComboProduct> ComboProducts { get; set; } = new List<ComboProduct>();
    }
}
