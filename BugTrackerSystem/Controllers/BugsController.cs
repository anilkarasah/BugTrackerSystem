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

        [HttpGet]
        public IActionResult GetAllBugs()
        {
            IEnumerable<Bug> allBugs = _bugService.GetBugs();

			if (allBugs is null || !allBugs.Any())
				return AppError(404, "No bugs found");

			return SendResponse(allBugs);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetBugByID(Guid id)
        {
            var bug = await _bugService.GetBugByID(id);

			if (bug is null)
				return AppError(404, "No bug found with given ID.");

			return SendResponse(_bugService.MapBugResponse(bug));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBug([FromBody] CreateBugRequest request)
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

            var createResponseMessage = await _bugService.CreateBug(bug);

			if (createResponseMessage.Equals("OK"))
				return CreatedAtAction(actionName: nameof(GetBugByID), routeValues: new { id = bug.ID }, value: _bugService.MapBugResponse(bug));
			else
				return AppError(500, createResponseMessage);
        }

		[HttpPatch("{id:Guid}")]
		public async Task<IActionResult> UpsertBug(Guid id, [FromBody] UpsertBugRequest request)
		{
			throw new NotImplementedException();
		}

		[HttpDelete("{id:Guid}")]
		public async Task<IActionResult> DeleteBug(Guid id)
		{
			throw new NotImplementedException();
		}
    }
}
