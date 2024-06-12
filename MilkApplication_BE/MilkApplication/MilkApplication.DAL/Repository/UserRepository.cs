using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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

        public async Task<ResponseDTO> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User retrieved successfully", Data = user };
            }

            return new ResponseDTO { IsSucceed = false, Message = "User not found" };
        }

        public async Task<ResponseDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User retrieved successfully", Data = user };
            }

            return new ResponseDTO { IsSucceed = false, Message = "User not found" };
        }

        public async Task<ResponseDTO> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return new ResponseDTO { IsSucceed = true, Message = "Users retrieved successfully", Data = users };
        }

        public async Task<ResponseDTO> UpdateUsersAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User updated successfully", Data = user };
            }

            return new ResponseDTO { IsSucceed = false, Message = "User update failed", Data = result.Errors };
        }

        public async Task<ResponseDTO> DeleteUserAsync(ApplicationUser user)
        {
            user.Status = UserStatus.Disable;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User status changed to disable successfully" };
            }

            return new ResponseDTO { IsSucceed = false, Message = "Changing user status failed", Data = result.Errors };
        }


        public async Task<ResponseDTO> RoleExistsAsync(string roleName)
        {
            var exists = await _roleManager.RoleExistsAsync(roleName);
            return new ResponseDTO { IsSucceed = exists, Message = exists ? "Role exists" : "Role does not exist" };
        }

        public async Task<ResponseDTO> CreateRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "Role created successfully" };
                }

                return new ResponseDTO { IsSucceed = false, Message = "Role creation failed", Data = result.Errors };
            }

            return new ResponseDTO { IsSucceed = false, Message = "Role already exists" };
        }

        public async Task<ResponseDTO> AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User added to role successfully" };
            }

            return new ResponseDTO { IsSucceed = false, Message = "Adding user to role failed", Data = result.Errors };
        }

        public async Task<ResponseDTO> GetUserRolesAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new ResponseDTO { IsSucceed = true, Message = "User roles retrieved successfully", Data = roles };
        }
    }
}
