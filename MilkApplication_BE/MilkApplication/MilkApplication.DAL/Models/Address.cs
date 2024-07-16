using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class Address
    {
        [Key]
        public int addressId { get; set; }
        public string Phone { get; set;}
        public string AddressName { get; set; } 
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string? Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser? User { get; set; }
    }
}
