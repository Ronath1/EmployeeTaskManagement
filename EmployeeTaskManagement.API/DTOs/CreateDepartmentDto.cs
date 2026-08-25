using System.ComponentModel.DataAnnotations;


namespace EmployeeTaskManagement.API.DTOs
{
    public class CreateDepartmentDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
