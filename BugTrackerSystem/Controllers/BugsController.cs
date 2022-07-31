

namespace BugTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly DataContext _context;
        public BugsController(DataContext context)
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
        public async Task<ActionResult<List<Bug>>> GetBug(Guid id)
        {
            var bug = await _context.Bugs.FindAsync(id);
            if (bug is null)
                return NotFound("No bug found with the given ID.");

            return Ok(bug);
        }

        [HttpPost]
        public async Task<ActionResult<Bug>> CreateBug([FromBody] Bug b)
        {
            b.CreatedAt = DateTime.Now;
            _context.Bugs.Add(b);
            await _context.SaveChangesAsync();

            return Ok(b);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Bug>> ChangeStatusOfBug(Guid id, [FromBody] int statusID)
        {
            var bug = await _context.Bugs.FindAsync(id);

            if (bug is null)
                return NotFound("No bug found with given ID.");

            bug.TrackStatus = await _context.Status.FindAsync(statusID);

            if (bug.TrackStatus is null)
                return NotFound("No status is found with gived ID.");

            await _context.SaveChangesAsync();

            return Ok(bug);
        }
    }
}
