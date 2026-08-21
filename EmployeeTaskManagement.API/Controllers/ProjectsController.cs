using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<List<Project>> GetProjects()
        {
            var projects = _context.Projects.Include(p => p.Manager).ToList();
            return Ok(projects);
        }



        [HttpGet("{id}")]
        public ActionResult<Project> GetProjectById(int id)
        {
            var project = _context.Projects.Include(p => p.Manager).FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);

        }

        [HttpPost]
        public ActionResult<Project> CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);


        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, Project updatedProject)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            project.Name = updatedProject.Name;
            project.Description = updatedProject.Description;
            project.StartDate = updatedProject.StartDate;
            project.EndDate = updatedProject.EndDate;
            project.Status = updatedProject.Status;
            project.ManagerId = updatedProject.ManagerId;

            _context.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id}")]

        public ActionResult DeleteProject(int id)
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
