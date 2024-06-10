using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilkApplication.BLL.Service;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MilkApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IAuthService userService, JwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Status = UserStatus.IsActive
            };

            var result = await _userService.RegisterUserAsync(user, model.Password, UserRole.User);
            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var userResponse = await _userService.GetUserByEmailAsync(model.Email);
            if (!userResponse.IsSucceed)
            {
                return Unauthorized(userResponse.Message);
            }

            var user = userResponse.Data as ApplicationUser;
            if (user.Status != UserStatus.IsActive)
            {
                return Unauthorized("User account is disabled");
            }

            var passwordResponse = await _userService.ValidateUserAsync(user, model.Password);
            if (!passwordResponse.IsSucceed)
            {
                return Unauthorized(passwordResponse.Message);
            }

            var token = _jwtHelper.GenerateJwtToken(user);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userService.UpdateUserAsync(user);

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (tokenDTO == null)
            {
                return BadRequest("Invalid client request");
            }

            var principal = _jwtHelper.GetPrincipalFromExpiredToken(tokenDTO.Token);
            if (principal == null)
            {
                return BadRequest("Invalid token");
            }

            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userResponse = await _userService.GetUserByIdAsync(userId);

            if (!userResponse.IsSucceed)
            {
                return Unauthorized(userResponse.Message);
            }

            var user = userResponse.Data as ApplicationUser;

            if (user == null || user.RefreshToken != tokenDTO.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized("Invalid refresh token");
            }

            var newAccessToken = _jwtHelper.GenerateJwtToken(user);
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userService.UpdateUserAsync(user);

            return Ok(new { Token = newAccessToken, RefreshToken = newRefreshToken });
        }


    }

}
