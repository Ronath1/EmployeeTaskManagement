using Microsoft.AspNetCore.Mvc;
using EmployeeTaskManagement.API.Models;

namespace EmployeeTaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  // This tells ASP.NET Core this class is an API controller and enables useful API behavior.

    public class EmployeesController : ControllerBase  //  This gives us methods like Ok(), NotFound(), and BadRequest().
    {
        private static readonly List<Employee> Employees = new()  //  This is temporary fake data stored in memory. Important: when you stop the app, this data resets. Later, SQL Server will store data permanently.
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
       };
        [HttpGet]  // This means this method handles HTTP GET requests.
        public ActionResult<List<Employee>> GetEmployees()  // This means the method returns an HTTP response containing a list of employees.
        {

            return Ok(Employees); // This returns HTTP status code 200 OK with the employee list.
        }
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            var employee = Employees.FirstOrDefault(e => e.Id == id);  //This is a LINQ method(FirstOrDefault). It searches the list and returns the first employee where:e.Id == id. If no employee is found, it returns null.
            if (employee == null)
            {
                return NotFound(); // This returns HTTP status code 404 Not Found if the employee is not found.
            }
            return Ok(employee); // This returns HTTP status code 200 OK with the employee data.
        }

        [HttpPost]
        public ActionResult<Employee> CreateEmployees(Employee employee) //ASP.NET Core reads the JSON body from the request and converts it into an Employee object.
        {
            employee.Id = Employees.Max(e => e.Id) + 1;
            Employees.Add(employee);

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);  // This returns HTTP status code 201 Created with the location of the newly created employee.
        }

        [HttpPut("{id}")]

        public IActionResult UpdateEmployee(int id, Employee updatedEmployee) //This means the method can return different HTTP responses, like: 404 Not Found  or  204 No Content  
                                                                              // updatedEmployee <-This comes from the JSON body of the request.
        {
            var employee = Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Email = updatedEmployee.Email;
            employee.Phone = updatedEmployee.Phone;
            employee.Position = updatedEmployee.Position;
            employee.HireDate = updatedEmployee.HireDate;

            return NoContent(); // This returns HTTP status code 204 No Content to indicate the update was successful.
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
        var employee = Employees.FirstOrDefault(e => e.Id == id);
        if(employee == null)
        {
                return NotFound();
        }

            Employees.Remove(employee);

            return NoContent();


        }

    }
}
