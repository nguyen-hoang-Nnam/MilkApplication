using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
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

        public UserRepository(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
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
    }
}
