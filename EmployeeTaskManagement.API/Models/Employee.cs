namespace EmployeeTaskManagement.API.Models
{
    public class Employee
    {
      
    public int Id { get; set; }

    public string FirstName { get; set;  } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } =string.Empty;

    public string? Phone { get; set; }     // ? thsi means it can be null. phoen number can be null

    public string Position { get; set; } =string.Empty;

    public DateTime HireDate { get; set; }

    }
}
