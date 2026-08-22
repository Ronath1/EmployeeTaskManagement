using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.API.DTOs
{
    public class UpdateEmployeeDto

    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email{ get; set; } = string.Empty;

        public string? Phone { get; set; }
        [Required]

        public string Position { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public int? DepartmentId { get; set;}

    }
}
