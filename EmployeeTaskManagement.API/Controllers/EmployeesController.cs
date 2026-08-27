using Microsoft.AspNetCore.Mvc;

using EmployeeTaskManagement.API.DTOs;
using EmployeeTaskManagement.API.Services;

namespace EmployeeTaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  // This tells ASP.NET Core this class is an API controller and enables useful API behavior.

    public class EmployeesController : ControllerBase  //  This gives us methods like Ok(), NotFound(), and BadRequest().
    {
        /*  private static readonly List<Employee> Employees = new()  //  This is temporary fake data stored in memory. Important: when you stop the app, this data resets. Later, SQL Server will store data permanently.
         {

           new Employee
           {
           Id= 1,
           FirstName = "John",
           LastName = "smith",
           Email = "John.smith@example.com",
           Phone = "0702001245",
           Position = "software engineer",
           HireDate = new DateTime(2024, 1, 10)


           },
           new Employee
           {
                  Id = 2,
                  FirstName = "Sarah",
                  LastName = "Johnson",
                  Email = "sarah.johnson@example.com",
                  Phone = "0779876543",
                  Position = "Project Manager",
                  HireDate = new DateTime(2023, 8, 15)
              }
         };  */


        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }




        [HttpGet]
        public ActionResult<PagedResultDto<EmployeeDto>> GetEmployees(
    string? search,
    int? departmentId,
    string? position,
    string? sortBy,
    string? sortOrder,
    int pageNumber = 1,
    int pageSize = 10)
        {
            var result = _employeeService.GetEmployees(
                search,
                departmentId,
                position,
                sortBy,
                sortOrder,
                pageNumber,
                pageSize);

            return Ok(result);
        }



        [HttpGet("{id}")]
        public ActionResult<EmployeeDto> GetEmployeeById(int id)
        {
            var employee = _employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }




        [HttpPost]
        public ActionResult<EmployeeDto> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var result = _employeeService.CreateEmployee(createEmployeeDto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = result.Data!.Id },
                result.Data);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var result = _employeeService.UpdateEmployee(id, updateEmployeeDto);

            if (!result.Success)
            {
                if (result.Message == "Employee not found.")
                {
                    return NotFound(result.Message);
                }

                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var deleted = _employeeService.DeleteEmployee(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
