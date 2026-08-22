using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeTaskManagement.API.Models;
using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.DTOs;

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


        private readonly AppDbContext _context;  //This stores the database context inside the controller.
        public EmployeesController(AppDbContext context)  //This is called constructor injection. ASP.NET Core automatically provides an instance of AppDbContext when it creates the controller.
        {
            _context = context; //This is the object we will use to read/write employees from SQL Server.
        }




        [HttpGet]  // This means this method handles HTTP GET requests.
        public ActionResult<List<EmployeeDto>> GetEmployees()
        {
        var employees = _context.Employees.Include(e =>e.Department).Select(e=>new  EmployeeDto
        {
        Id=e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            Position = e.Position,
            HireDate = e.HireDate,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department != null ? e.Department.Name : null
        }
        ) .ToList();

         return Ok(employees);  // This returns HTTP status code 200 OK with the list of employees.
        }



        [HttpGet("{id}")]
        public ActionResult<EmployeeDto> GetEmployeeById(int id)
        {
            var employee = _context.Employees.Include(e => e.Department).Where(e => e.Id == id).Select(e => new EmployeeDto
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

            if(employee == null)
            {
            return NotFound();
            }

            return Ok(employee);  // This returns HTTP status code 200 OK with the employee data if found, or 404 Not Found if not found.
        }




        [HttpPost]
        public ActionResult<EmployeeDto> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
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
            _context.SaveChanges();  //This saves the new employee to the database. Until this is called, the employee is not actually added to SQL Server.

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
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employeeDto);  // This returns HTTP status code 201 Created with the location of the new employee and the employee data.
        }





        [HttpPut("{id}")]

        public IActionResult UpdateEmployee(int id, UpdateEmployeeDto updatedEmployeeDto)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if(employee == null)
            {
                return NotFound();
            }

            employee.FirstName = updatedEmployeeDto.FirstName;
            employee.LastName = updatedEmployeeDto.LastName;
            employee.Email = updatedEmployeeDto.Email;
            employee.Phone = updatedEmployeeDto.Phone;
            employee.Position = updatedEmployeeDto.Position;
            employee.HireDate = updatedEmployeeDto.HireDate;
            employee.DepartmentId = updatedEmployeeDto.DepartmentId;

            _context.SaveChanges();  //This saves the updated employee to the database.
            return NoContent();  // This returns HTTP status code 204 No Content, indicating the update was successful but there's no content to return.`
        }
        



        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
        if(employee == null)
        {
                return NotFound();
        }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return NoContent();


        }

    }
}
