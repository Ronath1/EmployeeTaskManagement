namespace EmployeeTaskManagement.API.DTOs
{
    public class ProjectDto
    {
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public int? ManagerId { get; set; }

    public string? ManagerName { get; set; }

    public int TaskCount { get; set; }
    }
}
