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
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            var response = await _userService.CreateUserAsync(userDto, UserRole.Staff);
            if (response.IsSucceed)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
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

        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDTO user)
        {
            var response = await _userService.UpdateUserAsync(userId, user);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var response = await _userService.DeleteUserAsync(userId);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("UpdatePassword/{email}")]
        public async Task<IActionResult> UpdatePassword(string email, [FromBody] UpdatePasswordDTO updatePasswordDto)
        {
            var result = await _userService.UpdateUserPasswordAsync(email, updatePasswordDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }

}
