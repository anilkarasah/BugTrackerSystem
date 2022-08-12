using BugTrackerAPI.Contracts.Bugs;

namespace BugTrackerAPI.Controllers;

public class BugsController : ApiController
{
	private readonly IBugService _bugService;
	public BugsController(IBugService bugService)
	{
		_bugService = bugService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllBugs()
	{
		var allBugs = await _bugService.GetBugs();
		return SendResponse(allBugs);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetBugByID(Guid id)
	{
		var bug = await _bugService.GetBugByID(id);
		return SendResponse(_bugService.MapBugResponse(bug));
	}

	[HttpPost]
	public async Task<IActionResult> CreateBug(CreateBugRequest request)
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

		return CreatedAtAction(actionName: nameof(GetBugByID), routeValues: new { id = bug.ID }, value: _bugService.MapBugResponse(bug));
	}

	[HttpPatch("{id:Guid}")]
	public async Task<IActionResult> UpsertBug(Guid id, UpsertBugRequest request)
	{
		var bug = await _bugService.GetBugByID(id);

		bug.Title = request.Title ?? bug.Title;
		bug.Description = request.Description ?? bug.Description;
		bug.LogFile = request.LogFile ?? bug.LogFile;

		await _bugService.UpsertBug(bug);
		return SendResponse(bug);
	}

	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> DeleteBug(Guid id)
	{
		await _bugService.DeleteBug(id);
		return SendResponse(null, 204);
	}
}
