using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilkApplication.BLL.Service;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.Helpers;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MilkApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtHelper _jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, JwtHelper jwtHelper, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _jwtHelper = jwtHelper;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                Status = UserStatus.IsActive
            };

            var result = await _authService.RegisterUserAsync(user, model.Password, UserRole.User);
            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var userResponse = await _authService.GetUserByUserNameAsync(model.UserName);
            if (!userResponse.IsSucceed)
            {
                return Unauthorized(userResponse.Message);
            }

            var user = userResponse.Data as ApplicationUser;
            if (user.Status != UserStatus.IsActive)
            {
                return Unauthorized("User account is disabled");
            }

            var passwordResponse = await _authService.ValidateUserAsync(user, model.Password);
            if (!passwordResponse.IsSucceed)
            {
                return Unauthorized(passwordResponse.Message);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault();
            var token = _jwtHelper.GenerateJwtToken(user, userRole);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _authService.UpdateUserAsync(user);

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
            var userResponse = await _authService.GetUserByIdAsync(userId);

            if (!userResponse.IsSucceed)
            {
                return Unauthorized(userResponse.Message);
            }

            var user = userResponse.Data as ApplicationUser;

            if (user == null || user.RefreshToken != tokenDTO.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized("Invalid refresh token");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault();
            var newAccessToken = _jwtHelper.GenerateJwtToken(user, userRole);
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _authService.UpdateUserAsync(user);

            return Ok(new { Token = newAccessToken, RefreshToken = newRefreshToken });
        }


    }

}
