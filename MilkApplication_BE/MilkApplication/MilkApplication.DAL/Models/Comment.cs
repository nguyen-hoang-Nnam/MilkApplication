using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class Comment
    {
        [Key]
        public int commentId { get; set; }
        public string commentDetail { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow.AddHours(7);
        public int? productId { get; set; }
        [ForeignKey("productId")]
        public Product? Product { get; set; }
        public string? Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser? User { get; set; }

    }
}
