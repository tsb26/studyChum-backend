using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyChumAPI.Context;
using StudyChumAPI.Models;
using System.Security.Claims;

namespace StudyChumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        // GET all tasks for a specific date
        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetTasksByDate(DateTime date)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tasks = await _context.Tasks
                                      .Where(t => t.UserID == int.Parse(userId) && t.Date.Date == date.Date)
                                      .ToListAsync();
            return Ok(tasks);
        }

        // POST a new task
        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] UserTask task)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            task.UserID = int.Parse(userId); // Set the user ID to the current user

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return Ok(task);  // Simple OK response with the task object

        }

        // PUT to update an existing task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UserTask task)
        {
            if (id != task.TaskID)
            {
                return BadRequest();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (task.UserID != int.Parse(userId))
            {
                return Unauthorized(); // Ensure the task belongs to the current user
            }

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

            // DELETE a task
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteTask(int id)
            {
                var task = await _context.Tasks.FindAsync(id);
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (task == null || task.UserID != int.Parse(userId))
                {
                    return NotFound();
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        
    }
}
