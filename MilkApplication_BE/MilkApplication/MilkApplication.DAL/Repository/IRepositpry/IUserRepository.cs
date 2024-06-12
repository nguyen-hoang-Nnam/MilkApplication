using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IUserRepository
    {
        Task<ResponseDTO> CreateUserAsync(ApplicationUser user, string password);
        Task<ResponseDTO> GetUserByIdAsync(string userId);
        Task<ResponseDTO> GetUserByEmailAsync(string email);
        Task<ResponseDTO> GetAllUsersAsync();
        Task<ResponseDTO> UpdateUsersAsync(ApplicationUser user);
        Task<ResponseDTO> RoleExistsAsync(string roleName);
        Task<ResponseDTO> CreateRoleAsync(string roleName);
        Task<ResponseDTO> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<ResponseDTO> GetUserRolesAsync(ApplicationUser user);
        Task<ResponseDTO> DeleteUserAsync(ApplicationUser user);
    }
}
