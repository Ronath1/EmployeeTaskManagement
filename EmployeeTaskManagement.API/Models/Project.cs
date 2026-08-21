using System.ComponentModel.DataAnnotations;


namespace EmployeeTaskManagement.API.Models
{
    public class Project

    {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } =string.Empty;

    public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public string Status { get; set; } = "Planning";

        public int? ManagerId { get; set; }

        public Employee? Manager { get; set; }
    }
}
