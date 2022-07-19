using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly DataContext _context;

        public BugController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Bug>>> GetAllBugs()
        {
            var bugsList = await _context.Bugs.ToListAsync();
            if (bugsList.Count == 0)
                return NotFound("No bugs found!");

            return Ok(bugsList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Bug>>> GetBug(int id)
        {
            var bug = await _context.Bugs.FindAsync(id);
            if (bug == null)
                return NotFound("No bug found with the given ID.");

            return Ok(bug);
        }

        [HttpPost]
        public async Task<ActionResult<Bug>> CreateBug(Bug b)
        {
            _context.Bugs.Add(b);
            await _context.SaveChangesAsync();

            return Ok(b);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Bug>> ChangeStatusOfBug(int id, [FromBody]string status)
        {
            var bug = await _context.Bugs.FindAsync(id);
            if (bug == null)
                return NotFound("No bug found with given ID.");

            if (status.ToLower().Equals("tracking"))
                bug.TrackStatus = Status.Tracking;
            else if (status.ToLower().Equals("solved"))
                bug.TrackStatus = Status.Solved;
            else
                return BadRequest("Please provide a valid status for bugs! Options are 'tracking' and 'solved'");

            await _context.SaveChangesAsync();

            return Ok(bug);
        }
    }
}
