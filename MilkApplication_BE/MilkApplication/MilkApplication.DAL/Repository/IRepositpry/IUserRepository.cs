using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ResponseDTO> CreateUserAsync(ApplicationUser user, string password);
        Task<ResponseDTO> DeleteUserAsync(string userId, UserStatus status);
        Task<List<ApplicationUser>> GetUsersByStaffRoleAsync();
        Task<List<ApplicationUser>> GetUsersByAdminRoleAsync();
    }
}
