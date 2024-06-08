using Microsoft.AspNetCore.Identity;
using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public UserStatus Status { get; set; }
    }
}
