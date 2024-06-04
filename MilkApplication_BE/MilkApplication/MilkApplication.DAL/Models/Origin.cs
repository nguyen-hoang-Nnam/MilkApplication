using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class Origin
    {
        [Key]
        public int originId { get; set; }
        public string originName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
