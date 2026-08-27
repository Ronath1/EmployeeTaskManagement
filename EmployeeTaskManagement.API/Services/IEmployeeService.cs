using EmployeeTaskManagement.API.DTOs;

namespace EmployeeTaskManagement.API.Services
{
    public interface IEmployeeService
    {
        PagedResultDto<EmployeeDto> GetEmployees(
            string? search,
            int? departmentId,
            string? position,
            string? sortBy,
            string? sortOrder,
            int pageNumber,
            int pageSize);

        EmployeeDto? GetEmployeeById(int id);

        EmployeeDto CreateEmployee(CreateEmployeeDto createEmployeeDto);

        bool UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto);

        bool DeleteEmployee(int id);
    }
}