﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class Order
    {
        [Key]
        public int orderId { get; set; }
        public DateTime orderDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal totalPrice { get; set; }
        public string? Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser? User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
