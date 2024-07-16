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

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetById(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ResponseDTO> CreateUserAsync(ApplicationUser user, string password);
        Task<ResponseDTO> CreateStaffAsync(ApplicationUser user, string password);
        Task<ResponseDTO> CreateAdminAsync(ApplicationUser user, string password);
        Task<ResponseDTO> DeleteUserAsync(string userId, UserStatus status);
        Task<List<ApplicationUser>> GetUsersByStaffRoleAsync();
        Task<List<ApplicationUser>> GetUsersByAdminRoleAsync();
        public Task<Pagination<ApplicationUser>> GetAccountByFilterAsync(PaginationParameter paginationParameter, AccountFilterDTO accountFilterDTO);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
