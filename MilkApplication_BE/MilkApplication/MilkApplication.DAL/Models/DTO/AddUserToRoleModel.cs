using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class AddUserToRoleModel
    {
        public string Id { get; set; }
        public UserRole Role { get; set; }
    }
}
