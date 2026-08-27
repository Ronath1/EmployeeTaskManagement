namespace EmployeeTaskManagement.API.DTOs
{
    public class ServiceResultDto<T>
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}