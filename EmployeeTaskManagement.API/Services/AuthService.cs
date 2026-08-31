using EmployeeTaskManagement.API.Authentication;
using EmployeeTaskManagement.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeTaskManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ServiceResultDto<string>> Register(RegisterDto registerDto)
        {
            var allowedRoles = new[] { "Admin", "Manager", "Employee" };

            if (!allowedRoles.Contains(registerDto.Role))
            {
                return new ServiceResultDto<string>
                {
                    Success = false,
                    Message = "Invalid role."
                };
            }

            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);

            if (userExists != null)
            {
                return new ServiceResultDto<string>
                {
                    Success = false,
                    Message = "User with this email already exists."
                };
            }

            if (!await _roleManager.RoleExistsAsync(registerDto.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerDto.Role));
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));

                return new ServiceResultDto<string>
                {
                    Success = false,
                    Message = errors
                };
            }

            await _userManager.AddToRoleAsync(user, registerDto.Role);

            return new ServiceResultDto<string>
            {
                Success = true,
                Data = "User registered successfully."
            };
        }
        public async Task<ServiceResultDto<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return new ServiceResultDto<AuthResponseDto>
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordValid)
            {
                return new ServiceResultDto<AuthResponseDto>
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = GenerateJwtToken(user, roles, expiresAt);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                FullName = user.FullName,
                Role = roles.FirstOrDefault() ?? string.Empty,
                ExpiresAt = expiresAt
            };

            return new ServiceResultDto<AuthResponseDto>
            {
                Success = true,
                Data = response
            };
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles, DateTime expiresAt)
        {
            var claims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.Name, user.FullName)
         };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secret = _configuration["Jwt:Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResultDto<CurrentUserDto>> GetCurrentUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResultDto<CurrentUserDto>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var currentUser = new CurrentUserDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Role = roles.FirstOrDefault() ?? string.Empty
            };

            return new ServiceResultDto<CurrentUserDto>
            {
                Success = true,
                Data = currentUser
            };
        }
    }
}