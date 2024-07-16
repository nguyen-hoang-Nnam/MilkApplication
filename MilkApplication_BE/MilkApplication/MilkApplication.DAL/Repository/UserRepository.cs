using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class UserRepository: GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ResponseDTO> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User created successfully", Data = user };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User creation failed", Data = result.Errors };
        }
        public async Task<ResponseDTO> CreateStaffAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User created successfully", Data = user };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User creation failed", Data = result.Errors };
        }
        public async Task<ResponseDTO> CreateAdminAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User created successfully", Data = user };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User creation failed", Data = result.Errors };
        }
        public async Task<ApplicationUser> GetById(string userId)
        {
            return await _context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Addresses).ToListAsync();
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<ResponseDTO> DeleteUserAsync(string userId, UserStatus status)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "User not found" };
            }
            user.Status = UserStatus.Disable;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User status changed successfully"};
            }
            else
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed to change user status" };
            }
        }
        public async Task<List<ApplicationUser>> GetUsersByStaffRoleAsync()
        {
            var roleName = UserRole.Staff.ToString();
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.ToList();
        }
        public async Task<List<ApplicationUser>> GetUsersByAdminRoleAsync()
        {
            var roleName = UserRole.Admin.ToString();
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.ToList();
        }
        public async Task<List<ApplicationUser>> GetUsersExcludingStaffAndAdminAsync()
        {
            var users = await _context.Users.ToListAsync();
            var filteredUsers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (!roles.Contains(UserRole.Staff.ToString()) && !roles.Contains(UserRole.Admin.ToString()))
                {
                    filteredUsers.Add(user);
                }
            }

            return filteredUsers;
        }
        public async Task<Pagination<ApplicationUser>> GetAccountByFilterAsync(PaginationParameter paginationParameter, AccountFilterDTO accountFilterDTO)
        {
            try
            {
                var AccountsQuery = _context.Users.AsQueryable();
                AccountsQuery = await ApplyFilterSortAndSearch(AccountsQuery, accountFilterDTO);
                if (AccountsQuery != null)
                {
                    var AccountQuery = ApplySorting(AccountsQuery, accountFilterDTO);
                    var totalCount = await AccountQuery.CountAsync();

                    var AccountPagination = await AccountQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<ApplicationUser>(AccountPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<ApplicationUser>> ApplyFilterSortAndSearch(IQueryable<ApplicationUser> Query, AccountFilterDTO accountFilterDTO)
        {
            if (accountFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(accountFilterDTO.Search))
            {
                Query = Query.Where(x => x.Id.Contains(accountFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<ApplicationUser> ApplySorting(IQueryable<ApplicationUser> query, AccountFilterDTO accountFilterDTO)
        {
            switch (accountFilterDTO.Sort.ToLower())
            {
                case "FullName":
                    query = (accountFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName);
                    break;
                default:
                    query = (accountFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }
            return query;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
