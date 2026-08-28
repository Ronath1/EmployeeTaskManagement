using Microsoft.AspNetCore.Identity;

namespace EmployeeTaskManagement.API.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}