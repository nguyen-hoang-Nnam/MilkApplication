using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class Location
    {
        [Key]
        public int locationId { get; set; }
        public string locationName { get; set; }
        public string Address { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
