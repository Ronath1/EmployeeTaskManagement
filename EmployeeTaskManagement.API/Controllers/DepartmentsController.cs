using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Department>> GetDepartments()
        {
        var departments = _context.Departments
          .Include(d => d.Employees)
          .ToList();
            return Ok(departments);

        }

        [HttpGet("{id}")]
        public ActionResult<Department> GetDepartmentById(int id)
        {
            var department = _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost]
        public ActionResult<Department> CreateDepartment(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, Department updatedDepartment)
        {
        var department = _context.Departments.FirstOrDefault(d => d.Id == id);

        if (department == null)
            {
                return NotFound();
            }

            department.Name = updatedDepartment.Name;
            department.Description = updatedDepartment.Description;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
        var department = _context.Departments.FirstOrDefault(d => d.Id == id);  //Include(d => d.Employees) tells EF Core to also load employees related to each department.
            if (department == null)
            {
                return NotFound();
            }
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

