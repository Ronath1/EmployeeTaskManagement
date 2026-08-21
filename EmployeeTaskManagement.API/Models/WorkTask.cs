using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.API.Models
{
    public class WorkTask
    {
     public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } = "To Do";

        [Required]
        public string Priority { get; set; } = "Medium";

        public DateTime? DueDate { get; set; }

        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        public int? ProjectId { get; set; }

        public Project? Project { get; set; }


    }

}
