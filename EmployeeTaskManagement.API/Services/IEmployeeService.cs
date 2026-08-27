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

        ServiceResultDto<EmployeeDto> CreateEmployee(CreateEmployeeDto createEmployeeDto);

        ServiceResultDto<bool> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto);

        bool DeleteEmployee(int id);
    }
}