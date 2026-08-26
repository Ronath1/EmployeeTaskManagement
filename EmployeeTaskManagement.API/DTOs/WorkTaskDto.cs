namespace EmployeeTaskManagement.API.DTOs
{
    public class WorkTaskDto
    {
    public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public int? EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        public int? ProjectId { get; set; }

        public string? ProjectName { get; set; }
    }
}
