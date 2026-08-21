using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.Models;
using Microsoft.EntityFrameworkCore;


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

        public ActionResult<List<WorkTask>> GetWorkTasks()
        {
            var tasks = _context.WorkTasks.Include(t => t.Employee).Include(t => t.Project).ToList();

            return Ok(tasks);

        }

        [HttpGet("{id}")]

        public ActionResult<WorkTask> GetWorkTaskById(int id)
        {
            var tasks = _context.WorkTasks.Include(t => t.Employee).Include(t => t.Project).FirstOrDefault(t => t.Id == id);

            if(tasks == null)
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [HttpPost]
        public ActionResult<WorkTask> CreateWorkTask(WorkTask task)
        {
            _context.WorkTasks.Add(task);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetWorkTaskById), new { id = task.Id }, task);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateWorkTask(int id,WorkTask updatedTask)
        {
            var task = _context.WorkTasks.FirstOrDefault(t => t.Id == id);
            if(task  == null)
            {
                return NotFound();
            }
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status;
            task.Priority = updatedTask.Priority;
            task.DueDate = updatedTask.DueDate;
            task.EmployeeId = updatedTask.EmployeeId;
            task.ProjectId = updatedTask.ProjectId;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWorktask(int id)
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
