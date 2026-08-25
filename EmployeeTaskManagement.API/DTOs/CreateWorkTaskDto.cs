using System.ComponentModel.DataAnnotations;


namespace EmployeeTaskManagement.API.DTOs
{
    public class CreateWorkTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } = "To Do";

        [Required]
        public string Priority { get; set; } = "Medium";

        public DateTime? DueDate { get; set; }

        public int? EmployeeId { get; set; }

        public int? ProjectId { get; set; }


    }
}
