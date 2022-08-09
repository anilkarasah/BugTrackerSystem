using BugTrackerAPI.Contracts.Bugs;

namespace BugTrackerAPI.Controllers
{
    public class BugsController : ApiController
    {
        private readonly IBugService _bugService;
        public BugsController(IBugService bugService)
        {
            _bugService = bugService;
        }
        private static BugResponse MapBugResponse(Bug b)
        {
            return new BugResponse(
                b.ID,
                b.Title,
                b.Description,
                b.CreatedAt,
                b.Project,
                b.LogFile);
        }

        [HttpGet]
        public IActionResult GetAllBugs()
        {
            ErrorOr<IEnumerable<Bug>> allBugsResponse = _bugService.GetBugs();

            return allBugsResponse.Match(
                value => Ok(value),
                errors => Problem(errors));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetBugByID(Guid id)
        {
            var bug = await _bugService.GetBugByID(id);

            return bug.Match(
                value => Ok(MapBugResponse(value)),
                errors => Problem(errors));
        }

        [HttpPost]
        public async Task<ActionResult<Bug>> CreateBugAsync([FromBody] CreateBugRequest request)
        {
            var bug = new Bug
            {
                ID = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                ProjectID = request.ProjectID,
                LogFile = request.LogFile
            };

            await _bugService.CreateBug(bug);

            return CreatedAtAction(actionName: nameof(GetBugByID), routeValues: new { id = bug.ID }, value: bug);
        }
    }
}
