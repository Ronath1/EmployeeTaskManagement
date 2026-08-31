using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.DTOs;
using EmployeeTaskManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(AppDbContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public PagedResultDto<EmployeeDto> GetEmployees(
    string? search,
    int? departmentId,
    string? position,
    string? sortBy,
    string? sortOrder,
    int pageNumber,
    int pageSize)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search) ||
                    e.Email.Contains(search));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(position))
            {
                query = query.Where(e => e.Position.Contains(position));
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDescending = sortOrder?.ToLower() == "desc";

                query = sortBy.ToLower() switch
                {
                    "firstname" => isDescending
                        ? query.OrderByDescending(e => e.FirstName)
                        : query.OrderBy(e => e.FirstName),

                    "lastname" => isDescending
                        ? query.OrderByDescending(e => e.LastName)
                        : query.OrderBy(e => e.LastName),

                    "email" => isDescending
                        ? query.OrderByDescending(e => e.Email)
                        : query.OrderBy(e => e.Email),

                    "position" => isDescending
                        ? query.OrderByDescending(e => e.Position)
                        : query.OrderBy(e => e.Position),

                    "hiredate" => isDescending
                        ? query.OrderByDescending(e => e.HireDate)
                        : query.OrderBy(e => e.HireDate),

                    _ => query.OrderBy(e => e.Id)
                };
            }
            else
            {
                query = query.OrderBy(e => e.Id);
            }

            var totalCount = query.Count();

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var employees = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Position = e.Position,
                    HireDate = e.HireDate,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : null
                })
                .ToList();



            return new PagedResultDto<EmployeeDto>
            {
                Items = employees,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };


        }
        public EmployeeDto? GetEmployeeById(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Position = e.Position,
                    HireDate = e.HireDate,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : null
                })
                .FirstOrDefault();

            return employee;
        }

        public ServiceResultDto<EmployeeDto> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var emailExists = _context.Employees.Any(e => e.Email == createEmployeeDto.Email);

            if (emailExists)
            {
                return new ServiceResultDto<EmployeeDto>
                {
                    Success = false,
                    Message = "An employee with this email already exists."
                };
            }

            if (createEmployeeDto.DepartmentId.HasValue)
            {
                var departmentExists = _context.Departments
                    .Any(d => d.Id == createEmployeeDto.DepartmentId.Value);

                if (!departmentExists)
                {
                    return new ServiceResultDto<EmployeeDto>
                    {
                        Success = false,
                        Message = "The selected department does not exist."
                    };
                }
            }

            var employee = new Employee
            {
                FirstName = createEmployeeDto.FirstName,
                LastName = createEmployeeDto.LastName,
                Email = createEmployeeDto.Email,
                Phone = createEmployeeDto.Phone,
                Position = createEmployeeDto.Position,
                HireDate = createEmployeeDto.HireDate,
                DepartmentId = createEmployeeDto.DepartmentId
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();
            _logger.LogInformation("Employee created with ID {EmployeeId}", employee.Id);

            var employeeDto = new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                Position = employee.Position,
                HireDate = employee.HireDate,
                DepartmentId = employee.DepartmentId
            };

            return new ServiceResultDto<EmployeeDto>
            {
                Success = true,
                Data = employeeDto
            };
        }

        public ServiceResultDto<bool> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return new ServiceResultDto<bool>
                {
                    Success = false,
                    Message = "Employee not found."
                };
            }

            var emailExists = _context.Employees
                .Any(e => e.Email == updateEmployeeDto.Email && e.Id != id);

            if (emailExists)
            {
                return new ServiceResultDto<bool>
                {
                    Success = false,
                    Message = "Another employee with this email already exists."
                };
            }

            if (updateEmployeeDto.DepartmentId.HasValue)
            {
                var departmentExists = _context.Departments
                    .Any(d => d.Id == updateEmployeeDto.DepartmentId.Value);

                if (!departmentExists)
                {
                    return new ServiceResultDto<bool>
                    {
                        Success = false,
                        Message = "The selected department does not exist."
                    };
                }
            }

            employee.FirstName = updateEmployeeDto.FirstName;
            employee.LastName = updateEmployeeDto.LastName;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Position = updateEmployeeDto.Position;
            employee.HireDate = updateEmployeeDto.HireDate;
            employee.DepartmentId = updateEmployeeDto.DepartmentId;

            _context.SaveChanges();
            _logger.LogInformation("Employee updated with ID {EmployeeId}", employee.Id);

            return new ServiceResultDto<bool>
            {
                Success = true,
                Data = true
            };
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            _logger.LogInformation("Employee deleted with ID {EmployeeId}", id); // strucured logging

            return true;
        }

    }
}