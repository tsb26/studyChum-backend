using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyChumAPI.Context;
using StudyChumAPI.DTOs;
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
                             .Select(t => new UserTaskDTO
                             {
                                 TaskID = t.TaskID,
                                 Description = t.Description,
                                 IsCompleted = t.IsCompleted,
                                 Date = t.Date
                             })
                             .ToListAsync();
            return Ok(tasks);
        }

        // POST a new task
        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] UserTaskDTO task)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var newTask = new UserTask
            {
                UserID = int.Parse(userId),
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                Date = task.Date
            };
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();
            return Ok(new UserTaskDTO
            {
                TaskID = newTask.TaskID, 
                Description = newTask.Description,
                IsCompleted = newTask.IsCompleted,
                Date = newTask.Date
            });

        }

        // PUT to update an existing task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UserTaskDTO taskDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null || existingTask.UserID != int.Parse(userId))
            {
                return NotFound("Task not found or user unauthorized");
            }

            existingTask.Description = taskDto.Description;
            existingTask.IsCompleted = taskDto.IsCompleted;

            _context.Tasks.Update(existingTask);
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

        [HttpGet("tasks/completed/{startDate}/{endDate}")]
        public async Task<IActionResult> GetCompletedTasksCount(DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tasks = await _context.Tasks
                .Where(t => t.UserID == int.Parse(userId) && t.Date >= startDate && t.Date <= endDate && t.IsCompleted)
                .GroupBy(t => t.Date.Date)
                .Select(group => new { Date = group.Key, Count = group.Count() })
                .ToListAsync();

            return Ok(tasks);
        }


    }
}
