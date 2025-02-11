using Microsoft.AspNetCore.Authorization;
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
    public class PomodoroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PomodoroController(AppDbContext context)
        {
            _context = context;
        }

        // GET Pomodoro sessions count by date
        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetPomodoroCountByDate(DateTime date)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var pomodoroCount = await _context.PomodoroCounts
                                     .Where(p => p.UserID == int.Parse(userId) && p.Date.Date == date.Date)
                                     .FirstOrDefaultAsync();

            if (pomodoroCount == null)
                return Ok(new { SessionCount = 0 });  // Return zero count if no entry found

            return Ok(pomodoroCount);
        }

        // POST to add or update Pomodoro session count
        [HttpPost("update")]
        public async Task<IActionResult> AddOrUpdatePomodoroCount([FromBody] PomodoroCount updateData)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingEntry = await _context.PomodoroCounts
                                      .FirstOrDefaultAsync(p => p.UserID == int.Parse(userId) && p.Date.Date == updateData.Date.Date);

            if (existingEntry == null)
            {
                // Create new entry if it doesn't exist
                updateData.UserID = int.Parse(userId);
                _context.PomodoroCounts.Add(updateData);
            }
            else
            {
                // Update existing entry
                existingEntry.SessionCount += updateData.SessionCount;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("pomodoros/completed/{startDate}/{endDate}")]
        public async Task<IActionResult> GetCompletedPomodorosCount(DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var pomodoros = await _context.PomodoroCounts
                .Where(p => p.UserID == int.Parse(userId) && p.Date >= startDate && p.Date <= endDate)
                .GroupBy(p => p.Date)  // Group by date to get counts per day
                .Select(group => new {
                    Date = group.Key,    // The date of the Pomodoro sessions
                    Count = group.Sum(g => g.SessionCount)  // Sum all sessions for each day
                })
                .ToListAsync();

            return Ok(pomodoros);
        }

    }
}
