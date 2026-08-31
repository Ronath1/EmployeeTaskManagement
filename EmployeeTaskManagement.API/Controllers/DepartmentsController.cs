using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.Models;
using EmployeeTaskManagement.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<DepartmentDto>> GetDepartments()
        {
        var departments = _context.Departments
           .Select(d => new DepartmentDto
           {
               Id = d.Id,
               Name = d.Name,
               Description = d.Description,
               EmployeeCount = d.Employees.Count
           })
            .ToList();

            return Ok(departments);
        }
        

        [HttpGet("{id}")]
        public ActionResult<DepartmentDto> GetDepartmentById(int id)
        {
         var department = _context.Departments.Where(d  => d.Id == id).Select(d=> new DepartmentDto
         {
         Id = d.Id,
         Name=d.Name,
         Description= d.Description,
         EmployeeCount= d.Employees.Count
         })
         .FirstOrDefault();
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);  
        }

        [HttpPost]
        public ActionResult<DepartmentDto> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            var department = new Department
            {
                Name = createDepartmentDto.Name,
                Description = createDepartmentDto.Description

            };
            _context.Departments.Add(department);
            _context.SaveChanges();

            var departmentDto = new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                EmployeeCount = 0
            };

            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, departmentDto);
        }

        [HttpPut("{id}")]
        public IActionResult Updatedepartment(int id, UpdateDepartmentDto updateDepartmentdto)
        {
            var department = _context.Departments.FirstOrDefault(d => d.Id == id);
            if(department == null)
            {
            return NotFound();
            }

            department.Name = updateDepartmentdto.Name;
            department.Description = updateDepartmentdto.Description;

            _context.SaveChanges();

            return NoContent();
        }
        

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = _context.Departments.FirstOrDefault(d => d.Id == id);
            if(department ==null)
            {
            return NotFound();
            }

            _context.Departments.Remove(department);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

