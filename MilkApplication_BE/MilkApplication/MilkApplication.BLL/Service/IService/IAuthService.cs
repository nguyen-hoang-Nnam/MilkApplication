using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.enums;

namespace MilkApplication.BLL.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO> GetUserByIdAsync(string userId);
        Task<ResponseDTO> GetUserByEmailAsync(string email);
        Task<ResponseDTO> GetUserByUserNameAsync(string userName);
        Task<ResponseDTO> RegisterUserAsync(ApplicationUser user, string password, UserRole role);
        Task<ResponseDTO> ValidateUserAsync(ApplicationUser user, string password);
        Task UpdateUserAsync(ApplicationUser user);
    }
}
