using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        static List<Bug> bugs = new List<Bug>
        {
            new Bug(1, "Cannot change title", "When I log in as an administrator, I cannot change the title!"),
            new Bug(3, "Cannot log in as a user", "I cannot log in as a user because it says that my IP address is not whitelisted!")
        };

        [HttpGet]
        public async Task<ActionResult<List<Bug>>> GetBugs()
        {
            if (bugs.Count == 0)
                return NotFound();

            return Ok(bugs);
        }

        [HttpPost]
        public async Task<ActionResult<Bug>> CreateBug(Bug b)
        {
            bugs.Add(b);
            return Ok(b);
        }

        [HttpPatch("{id}&{st}")]
        public async Task<ActionResult<Bug>> MarkBugAsTracking(int id, string st)
        {
            Bug? bugToBeMarked = bugs.Find(b => b.Id == id);

            if (bugToBeMarked == null)
                return NotFound();

            if (st.Equals("tracking"))
                bugToBeMarked.TrackStatus = Status.Tracking;
            else if (st.Equals("solved"))
                bugToBeMarked.TrackStatus = Status.Solved;
            else
                return BadRequest();

            return Ok(bugToBeMarked);
        }
    }
}
