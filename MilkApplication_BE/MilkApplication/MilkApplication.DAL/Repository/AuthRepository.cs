using Microsoft.AspNetCore.Identity;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.enums;
using Microsoft.EntityFrameworkCore;

namespace MilkApplication.DAL.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseDTO> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User found", Data = user };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User not found" };
        }

        public async Task<ResponseDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User found", Data = user };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User not found" };
        }
        public async Task<ResponseDTO> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User found", Data = user };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User not found" };
        }

        public async Task<ResponseDTO> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User created successfully" };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User creation failed", Data = result.Errors };
        }

        public async Task<ResponseDTO> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (isPasswordValid)
            {
                return new ResponseDTO { IsSucceed = true, Message = "Password is valid" };
            }
            return new ResponseDTO { IsSucceed = false, Message = "Invalid password" };
        }
        public async Task<ResponseDTO> AddUserToRoleAsync(ApplicationUser user, UserRole role)
        {
            var result = await _userManager.AddToRoleAsync(user, role.ToString());
            if (result.Succeeded)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User added to role successfully" };
            }
            return new ResponseDTO { IsSucceed = false, Message = "Failed to add user to role", Data = result.Errors };
        }
        public async Task UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }

}
