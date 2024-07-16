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
    public class Product
    {
        [Key]
        public int productId { get; set; }
        public string productName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        private decimal? _discountPrice;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? discountPrice 
        { get => _discountPrice;
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
                    discountPrice = Price * (1 - (decimal)(_discountPercent.Value / 100));
                }
                else
                {
                    discountPrice = null;
                }
            }
        }
        public string productDescription { get; set; }
        public string Image { get; set; }
        public List<string> ImagesCarousel { get; set; } = new List<string>();
        public int Quantity { get; set; }
        public ProductStatus Status { get; set; }
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public Category? Category { get; set; }

        public int originId { get; set; }
        [ForeignKey("originId")]
        public Origin? Origin { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public int locationId { get; set; }
        [ForeignKey("locationId")]
        public Location? Location { get; set; }
        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }
        public ICollection<ComboProduct> ComboProducts { get; set; } = new List<ComboProduct>();
    }
}
