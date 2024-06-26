﻿using MilkApplication.BLL.Service.IService;
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
using AutoMapper;
using MilkApplication.DAL.Repository.IRepositpry.UoW;

namespace MilkApplication.BLL.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHelper _jwtHelper;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JwtHelper jwtHelper, IMapper mapper, IUnitOfWork unitOfWork, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseDTO> CreateUserAsync(UserDTO userDto, UserRole role)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);
            user.RefreshToken = _jwtHelper.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            user.Status = UserStatus.IsActive;
            var createUserResult = await _unitOfWork.UserRepository.CreateUserAsync(user, userDto.Password);
            if (!createUserResult.IsSucceed)
            {
                return createUserResult;
            }

            var roleName = role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!createRoleResult.Succeeded)
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Failed to create role: " + string.Join(", ", createRoleResult.Errors.Select(e => e.Description)) };
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addToRoleResult.Succeeded)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed to add user to role: " + string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)) };
            }

            return new ResponseDTO { IsSucceed = true, Message = "User registered successfully" };
        }

        public async Task<ResponseDTO> GetUserByIdAsync(string userId)
        {
            var userResponse = await _unitOfWork.UserRepository.GetById(userId);
            if (userResponse != null)
            {
                var userDto = _mapper.Map<UserDTO>(userResponse);
                return new ResponseDTO { IsSucceed = true, Message = "User retrieved successfully", Data = userDto };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User not found" };
        }

        public async Task<ResponseDTO> GetUserByEmailAsync(string email)
        {
            var userResponse = await _unitOfWork.UserRepository.GetByEmailAsync(email);
            if (userResponse != null)
            {
                var userDto = _mapper.Map<UserDTO>(userResponse);
                return new ResponseDTO { IsSucceed = true, Message = "User retrieved successfully", Data = userDto };
            }
            return new ResponseDTO { IsSucceed = false, Message = "User not found" };
        }

        public async Task<ResponseDTO> GetAllUsersAsync()
        {
            var usersResponse = await _unitOfWork.UserRepository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDTO>>(usersResponse);
            return new ResponseDTO { IsSucceed = true, Message = "Users retrieved successfully", Data = userDtos };
        }

        public async Task<ResponseDTO> UpdateUserAsync(string userId, UserDTO userDto)
        {
            var existingUser = await _unitOfWork.UserRepository.GetById(userId);

            if (existingUser == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "User not found" };
            }

            // Update the existingUser entity with data from userDto
            _mapper.Map(userDto, existingUser);
            existingUser.NormalizedUserName = existingUser.UserName.ToUpper();
            existingUser.NormalizedEmail = existingUser.Email.ToUpper();
            try
            {
                var result = await _unitOfWork.UserRepository.UpdateAsync(existingUser);

                if (result)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "User updated successfully" };
                }
                else
                {
                    return new ResponseDTO { IsSucceed = false, Message = "User update failed" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseDTO { IsSucceed = false, Message = "An error occurred while updating user" };
            }
        }


        public async Task<ResponseDTO> DeleteUserAsync(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "User not found" };
            }

            user.Status = UserStatus.Disable;

            var result = await _unitOfWork.UserRepository.UpdateAsync(user);

            if (result)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User status changed to disable successfully" };
            }
            else
            {
                return new ResponseDTO { IsSucceed = false, Message = "Changing user status failed"};
            }
        }
        public async Task<ResponseDTO> UnbanUserAsync(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "User not found" };
            }

            user.Status = UserStatus.IsActive;

            var result = await _unitOfWork.UserRepository.UpdateAsync(user);

            if (result)
            {
                return new ResponseDTO { IsSucceed = true, Message = "User status changed to active successfully" };
            }
            else
            {
                return new ResponseDTO { IsSucceed = false, Message = "Changing user status failed" };
            }
        }
        public async Task<ResponseDTO> UpdateUserPasswordAsync(string email, UpdatePasswordDTO updatePasswordDto)
        {
            var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(email);

            if (existingUser == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Email not found" };
            }

            // Verify current password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, updatePasswordDto.CurrentPassword);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Current password is incorrect" };
            }

            // Update user's password
            existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, updatePasswordDto.NewPassword);
            existingUser.SecurityStamp = Guid.NewGuid().ToString(); // Change security stamp to invalidate existing tokens

            try
            {
                var result = await _unitOfWork.UserRepository.UpdateAsync(existingUser);

                if (result)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "Password updated successfully" };
                }
                else
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Password update failed" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseDTO { IsSucceed = false, Message = "An error occurred while updating password" };
            }
        }
        public async Task<ResponseDTO> GetUsersByStaffRoleAsync()
        {
            var staffResponse = await _unitOfWork.UserRepository.GetUsersByStaffRoleAsync();
            if (staffResponse != null && staffResponse.Any())
            {
                var staffDto = _mapper.Map<List<UserDTO>>(staffResponse);
                return new ResponseDTO { IsSucceed = true, Message = "Staff retrives successfully", Data = staffDto };
            }
            return new ResponseDTO { IsSucceed = false, Message = "Staff not found" };
        }
        public async Task<ResponseDTO> GetUsersByAdminRoleAsync()
        {
            var adminResponse = await _unitOfWork.UserRepository.GetUsersByAdminRoleAsync();
            if (adminResponse != null && adminResponse.Any())
            {
                var adminDto = _mapper.Map<List<UserDTO>>(adminResponse);
                return new ResponseDTO { IsSucceed = true, Message = "Admin retrives successfully", Data = adminDto };
            }
            return new ResponseDTO { IsSucceed = false, Message = "Admin not found" };
        }

    }

}
