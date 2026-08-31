using EmployeeTaskManagement.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using EmployeeTaskManagement.API.Services;

namespace EmployeeTaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);

            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<CurrentUserDto>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _authService.GetCurrentUser(userId);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }


        //private string GenerateJwtToken(ApplicationUser user, IList<string> roles, DateTime expiresAt)
        //{
        //    var claims = new List<Claim>
        //  {
        //new Claim(ClaimTypes.NameIdentifier, user.Id),
        //new Claim(ClaimTypes.Email, user.Email!),
        //new Claim(ClaimTypes.Name, user.FullName)
        //   };

        //    foreach (var role in roles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    var secret = _configuration["Jwt:Secret"];
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: expiresAt,
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


    }


}