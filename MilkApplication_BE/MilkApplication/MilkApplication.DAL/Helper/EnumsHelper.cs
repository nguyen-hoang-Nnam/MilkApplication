using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Helper
{
    public class EnumsHelper
    {
        public static UserRole ConvertToRoleEnum(string role)
        {
            Console.WriteLine(role);
            switch (role.ToLower())
            {
                case "admin":
                    return UserRole.Admin;
                case "staff":
                    return UserRole.Staff;
                case "user":
                    return UserRole.User;
                default:
                    throw new ArgumentException("Invalid role value.");
            }
        }

        public static int ConvertToRoleId(string role)
        {
            Console.WriteLine(role);
            switch (role.ToLower())
            {
                case "admin":
                    return 1;
                case "staff":
                    return 2;
                case "user":
                    return 3;
                default:
                    throw new ArgumentException("Invalid role value.");
            }
        }
    }
}
