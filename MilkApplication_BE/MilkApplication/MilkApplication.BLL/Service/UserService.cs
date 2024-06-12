using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.Repository.IRepositpry;
using Microsoft.AspNetCore.Identity;
using MilkApplication.Helpers;

namespace MilkApplication.BLL.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHelper _jwtHelper;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtHelper = jwtHelper;
        }

        public async Task<ResponseDTO> CreateUserAsync(ApplicationUser user, string password, UserRole role)
        {
            user.RefreshToken = _jwtHelper.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            var createUserResult = await _userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                // Handle user creation failure
                return new ResponseDTO { IsSucceed = false, Message = "Failed to create user: " + string.Join(", ", createUserResult.Errors.Select(e => e.Description)) };
            }

            // Check if the role exists, and if not, create it
            var roleName = role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var newRole = new IdentityRole(roleName);
                var createRoleResult = await _roleManager.CreateAsync(newRole);
                if (!createRoleResult.Succeeded)
                {
                    // Handle role creation failure
                    return new ResponseDTO { IsSucceed = false, Message = "Failed to create role: " + string.Join(", ", createRoleResult.Errors.Select(e => e.Description)) };
                }
            }

            // Add user to role
            var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addToRoleResult.Succeeded)
            {
                // Handle adding user to role failure
                return new ResponseDTO { IsSucceed = false, Message = "Failed to add user to role: " + string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)) };
            }

            return new ResponseDTO { IsSucceed = true, Message = "User registered successfully" };
        }

        public async Task<ResponseDTO> GetUserByIdAsync(string userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<ResponseDTO> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<ResponseDTO> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<ResponseDTO> UpdateUserAsync(ApplicationUser user)
        {
            return await _userRepository.UpdateUsersAsync(user);
        }

        public async Task<ResponseDTO> DeleteUserAsync(ApplicationUser user)
        {
            return await _userRepository.DeleteUserAsync(user);
        }

        public async Task<ResponseDTO> CreateRoleAsync(string roleName)
        {
            return await _userRepository.CreateRoleAsync(roleName);
        }

        public async Task<ResponseDTO> AddUserToRoleAsync(ApplicationUser user, UserRole role)
        {
            return await _userRepository.AddUserToRoleAsync(user, role.ToString());
        }
    }
}
