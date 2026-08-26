using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeTaskManagement.API.DTOs;


namespace EmployeeTaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkTasksController(AppDbContext context)
        {
            _context = context;

        }

        [HttpGet]

        public ActionResult<List<WorkTaskDto>> GetWorkTasks()

        { var tasks = _context.WorkTasks.Select(t => new WorkTaskDto
          {
          Id = t.Id,
          Title = t.Title,
          Description = t.Description,
          Status = t.Status,
          Priority = t.Priority,
          DueDate = t.DueDate,
          EmployeeId = t.EmployeeId,
          EmployeeName = t.Employee !=null
             ? t.Employee.FirstName + " " + t.Employee.LastName : null,
           ProjectId = t.ProjectId,
           ProjectName = t.Project != null? t.Project.Name : null
          }
        
        )
        .ToList();

            return Ok(tasks);
        }



        

        [HttpGet("{id}")]
        public ActionResult<WorkTaskDto> GetWorkTaskById(int id)
        {
            var task = _context.WorkTasks
                .Where(t => t.Id == id)
                .Select(t => new WorkTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    EmployeeId = t.EmployeeId,
                    EmployeeName = t.Employee != null
                        ? t.Employee.FirstName + " " + t.Employee.LastName
                        : null,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project != null ? t.Project.Name : null
                })
                .FirstOrDefault();

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public ActionResult<WorkTaskDto> CreateWorkTask(CreateWorkTaskDto createWorkTaskDto)
        {   

            if(createWorkTaskDto.EmployeeId.HasValue)
            {
                var employeeExists = _context.Employees.Any(e => e.Id == createWorkTaskDto.EmployeeId.Value);

                if (!employeeExists)
                {
                    return BadRequest("The selected employee doesnot exxist");
                }
                
            }

            if (createWorkTaskDto.ProjectId.HasValue)
            {
                var projectExists = _context.Projects.Any(p => p.Id == createWorkTaskDto.ProjectId.Value);

                if (!projectExists)
                {
                    return BadRequest("The selected project doesnot exist");
                }
            }

            var task = new WorkTask
            {
                Title = createWorkTaskDto.Title,
                Description = createWorkTaskDto.Description,
                Status = createWorkTaskDto.Status,
                Priority = createWorkTaskDto.Priority,
                DueDate = createWorkTaskDto.DueDate,
                EmployeeId = createWorkTaskDto.EmployeeId,
                ProjectId = createWorkTaskDto.ProjectId
            };

            _context.WorkTasks.Add(task);
            _context.SaveChanges();

            var taskDto = new WorkTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                EmployeeId = task.EmployeeId,
                ProjectId = task.ProjectId
            };

            return CreatedAtAction(nameof(GetWorkTaskById), new { id = task.Id }, taskDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateWorkTask(int id, UpdateWorkTaskDto updateWorkTaskDto)
        {
            var task = _context.WorkTasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            if (updateWorkTaskDto.EmployeeId.HasValue)
            {
                var employeeExists = _context.Employees
                    .Any(e => e.Id == updateWorkTaskDto.EmployeeId.Value);

                if (!employeeExists)
                {
                    return BadRequest("The selected employee does not exist.");
                }
            }

            if (updateWorkTaskDto.ProjectId.HasValue)
            {
                var projectExists = _context.Projects
                    .Any(p => p.Id == updateWorkTaskDto.ProjectId.Value);

                if (!projectExists)
                {
                    return BadRequest("The selected project does not exist.");
                }
            }

            task.Title = updateWorkTaskDto.Title;
            task.Description = updateWorkTaskDto.Description;
            task.Status = updateWorkTaskDto.Status;
            task.Priority = updateWorkTaskDto.Priority;
            task.DueDate = updateWorkTaskDto.DueDate;
            task.EmployeeId = updateWorkTaskDto.EmployeeId;
            task.ProjectId = updateWorkTaskDto.ProjectId;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWorkTask(int id)
        {
            var task = _context.WorkTasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            _context.WorkTasks.Remove(task);
            _context.SaveChanges();

            return NoContent();
        }



    }
}
