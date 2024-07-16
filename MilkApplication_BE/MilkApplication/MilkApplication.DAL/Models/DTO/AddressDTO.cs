using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
        public string Phone { get; set; }
        public string AddressName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Id { get; set; }
    }
}
