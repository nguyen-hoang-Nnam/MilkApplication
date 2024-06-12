using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using MilkApplication.BLL.Service.IService;
using MilkApplication.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MilkApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtHelper _jwtHelper;

        public UsersController(IUserService userService, JwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            var user = new ApplicationUser 
            {   
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email,
                Status = UserStatus.IsActive
            };
            var response = await _userService.CreateUserAsync(user, model.Password, UserRole.Staff);

            if (!response.IsSucceed)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var response = await _userService.GetUserByEmailAsync(email);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            return Ok(response);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] ApplicationUser user)
        {
            var response = await _userService.UpdateUserAsync(user);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] ApplicationUser user)
        {
            var response = await _userService.DeleteUserAsync(user);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var response = await _userService.CreateRoleAsync(roleName);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleModel model)
        {
            var user = await _userService.GetUserByIdAsync(model.Id);
            if (!user.IsSucceed)
            {
                return NotFound(user);
            }

            var response = await _userService.AddUserToRoleAsync((ApplicationUser)user.Data, model.Role);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }

}
