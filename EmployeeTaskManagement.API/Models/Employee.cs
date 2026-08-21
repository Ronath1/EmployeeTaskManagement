using System.ComponentModel.DataAnnotations;  //This namespace contains validation attributes like [Required], [EmailAddress], [StringLength], and more.

namespace EmployeeTaskManagement.API.Models
{
    public class Employee
    {
      
    public int Id { get; set; }

    [Required]  //Means the field must be provided.
        public string FirstName { get; set;  } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]    //Means the value must follow an email-like format.
        public string Email { get; set; } =string.Empty;

    public string? Phone { get; set; }     // ? thsi means it can be null. phoen number can be null

    [Required]
    public string Position { get; set; } =string.Empty;

    public DateTime HireDate { get; set; }

    public int? DepartmentId { get; set; }  //This is a foreign key that links the employee to a department. It can be null if the employee is not assigned to any department.

    public Department? Department { get; set; }  //This is a navigation property that allows us to access the department details of the employee. It can be null if the employee is not assigned to any department.

    }
}
