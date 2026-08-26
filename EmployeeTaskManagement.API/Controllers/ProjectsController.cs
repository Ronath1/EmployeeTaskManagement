using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeTaskManagement.API.DTOs;

namespace EmployeeTaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult<List<ProjectDto>> GetProjects()
        {
        var projects = _context.Projects.Select(p => new ProjectDto
        {
          Id = p.Id,
          Name = p.Name,
          Description = p.Description,
          StartDate = p.StartDate,
          EndDate = p.EndDate,
          Status = p.Status,
          ManagerId = p.ManagerId,
          ManagerName = p.Manager !=null
               ? p.Manager.FirstName + " " + p.Manager.LastName
               : null,
               TaskCount = p.Tasks.Count

        })

        .ToList();

        return Ok(projects);
        }



        [HttpGet("{id}")]
        public ActionResult<ProjectDto> GetProjectById(int id)
        {
            var project = _context.Projects.Where(p => p.Id == id).Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                ManagerId = p.ManagerId,
                ManagerName = p.Manager != null
                    ? p.Manager.FirstName + " " + p.Manager.LastName
                    : null,
                TaskCount = p.Tasks.Count
            })
            .FirstOrDefault();

            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);

        }

        [HttpPost]
        public ActionResult<ProjectDto> CreateProject(CreateProjectDto createProjectDto)
        
        {


            if (createProjectDto.ManagerId.HasValue)
            {
                var managerExists = _context.Employees
                    .Any(e => e.Id == createProjectDto.ManagerId.Value);

                if (!managerExists)
                {
                    return BadRequest("The selected manager does not exist.");
                }
            }

            var project = new Project
            {
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
                StartDate = createProjectDto.StartDate,
                EndDate = createProjectDto.EndDate,
                Status = createProjectDto.Status,
                ManagerId = createProjectDto.ManagerId
            };
            _context.Projects.Add(project);
            _context.SaveChanges();

            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                ManagerId = project.ManagerId,
                TaskCount = 0
            };
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, projectDto);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, UpdateProjectDto updateProjectDto)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);

            if(project == null)
            {
            return NotFound();
            }

            if (updateProjectDto.ManagerId.HasValue)
            {
                var managerExists = _context.Employees.Any(e => e.Id == updateProjectDto.ManagerId.Value);

                if (!managerExists)
                {
                    return BadRequest("The selected amanger does not exist");
                }
            }

            project.Name = updateProjectDto.Name;
            project.Description = updateProjectDto.Description;
            project.StartDate = updateProjectDto.StartDate;
            project.EndDate = updateProjectDto.EndDate;
            project.Status = updateProjectDto.Status;
            project.ManagerId = updateProjectDto.ManagerId;
            _context.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id}")]

        public IActionResult DelteProject(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return NoContent();
        }

    }

}
