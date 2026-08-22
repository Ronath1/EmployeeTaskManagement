namespace EmployeeTaskManagement.API.DTOs
{
    public class EmployeeDto
    {
    public int Id { get; set; }

     public string FirstName { get; set; } = string.Empty;

     public string LastName { get; set; } = string.Empty;

     public string Email { get; set; } = string.Empty;

     public string? Phone { get; set; }

     public string Position { get; set; } = string.Empty;

     public DateTime HireDate { get; set; }

     public int? DepartmentId { get; set; }

     public string?  DepartmentName{ get; set;}


    }
}
