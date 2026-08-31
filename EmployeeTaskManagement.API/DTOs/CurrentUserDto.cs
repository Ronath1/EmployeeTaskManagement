namespace EmployeeTaskManagement.API.DTOs
{
    public class CurrentUserDto
    {
        public string UserId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}