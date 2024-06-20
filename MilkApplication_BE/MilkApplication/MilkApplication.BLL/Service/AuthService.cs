using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MilkApplication.Helpers;

namespace MilkApplication.BLL.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IAuthRepository authRepository, RoleManager<IdentityRole> roleManager, ILogger<AuthService> logger, UserManager<ApplicationUser> userManager, JwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
            _jwtHelper = jwtHelper;
        }

        public async Task<ResponseDTO> GetUserByIdAsync(string userId)
        {
            return await _authRepository.GetUserByIdAsync(userId);
        }

        public async Task<ResponseDTO> GetUserByEmailAsync(string email)
        {
            return await _authRepository.GetUserByEmailAsync(email);
        }
        public async Task<ResponseDTO> GetUserByUserNameAsync(string userName)
        {
            return await _authRepository.GetUserByUserNameAsync(userName);
        }

        public async Task<ResponseDTO> RegisterUserAsync(ApplicationUser user, string password, UserRole role)
        {
            // Generate refresh token and set expiry time
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


        public async Task<ResponseDTO> ValidateUserAsync(ApplicationUser user, string password)
        {
            return await _authRepository.CheckPasswordAsync(user, password);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await _authRepository.UpdateUserAsync(user);
        }
    }

}
