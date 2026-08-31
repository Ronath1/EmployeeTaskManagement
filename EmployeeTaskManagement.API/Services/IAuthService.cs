using EmployeeTaskManagement.API.DTOs;

namespace EmployeeTaskManagement.API.Services
{
    public interface IAuthService
    {
        Task<ServiceResultDto<string>> Register(RegisterDto registerDto);

        Task<ServiceResultDto<AuthResponseDto>> Login(LoginDto loginDto);

        Task<ServiceResultDto<CurrentUserDto>> GetCurrentUser(string userId);
    }
}