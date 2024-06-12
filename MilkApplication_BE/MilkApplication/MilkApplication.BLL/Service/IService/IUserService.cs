using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IUserService
    {
        Task<ResponseDTO> CreateUserAsync(ApplicationUser user, string password, UserRole role);
        Task<ResponseDTO> GetUserByIdAsync(string userId);
        Task<ResponseDTO> GetUserByEmailAsync(string email);
        Task<ResponseDTO> GetAllUsersAsync();
        Task<ResponseDTO> UpdateUserAsync(ApplicationUser user);
        Task<ResponseDTO> CreateRoleAsync(string roleName);
        Task<ResponseDTO> AddUserToRoleAsync(ApplicationUser user, UserRole role);
        Task<ResponseDTO> DeleteUserAsync(ApplicationUser user);
    }
}
