using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.API.DTOs
{
    public class UpdateDepartmentDto
    {
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    }
}
