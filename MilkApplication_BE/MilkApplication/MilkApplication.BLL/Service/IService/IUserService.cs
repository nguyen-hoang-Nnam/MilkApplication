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
        Task<ResponseDTO> CreateUserAsync(UserDTO userDto, UserRole role);
        Task<ResponseDTO> GetUserByIdAsync(string userId);
        Task<ResponseDTO> GetUserByEmailAsync(string email);
        Task<ResponseDTO> GetAllUsersAsync();
        Task<ResponseDTO> UpdateUserAsync(string userId,UserDTO userDto);
        Task<ResponseDTO> DeleteUserAsync(string userId);
        Task<ResponseDTO> UnbanUserAsync(string userId);
        Task<ResponseDTO> UpdateUserPasswordAsync(string email, UpdatePasswordDTO updatePasswordDto);
    }
}
