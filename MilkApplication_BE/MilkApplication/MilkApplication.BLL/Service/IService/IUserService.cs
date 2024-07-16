using MilkApplication.DAL.Commons;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
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
        Task<ResponseDTO> CreateStaffAsync(StaffDTO staffDto, UserRole role);
        Task<ResponseDTO> CreateAdminAsync(AdminDTO adminDto, UserRole role);
        Task<ResponseDTO> GetUserByIdAsync(string userId);
        Task<ResponseDTO> GetUserByEmailAsync(string email);
        Task<ResponseDTO> GetAllUsersAsync();
        Task<ResponseDTO> UpdateUserAsync(string userId,UserDTO userDto);
        Task<ResponseDTO> DeleteUserAsync(string userId);
        Task<ResponseDTO> UnbanUserAsync(string userId);
        Task<ResponseDTO> UpdateUserPasswordAsync(string email, UpdatePasswordDTO updatePasswordDto);
        Task<ResponseDTO> GetUsersByStaffRoleAsync();
        Task<ResponseDTO> GetUsersByAdminRoleAsync();
        public Task<Pagination<UserDTO>> GetAccountByFilterAsync(PaginationParameter paginationParameter, AccountFilterDTO accountFilterDTO);
    }
}
