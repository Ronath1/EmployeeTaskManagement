using System.ComponentModel.DataAnnotations;  //This namespace contains validation attributes like [Required], [EmailAddress], [StringLength], and more.


namespace EmployeeTaskManagement.API.Models
{
    public class Department
    {
    public int Id { get; set; }

    [Required]  //Means the field must be provided.
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<Employee> Employees { get; set; } = new();  
    }
    
}
