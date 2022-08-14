using BugTrackerAPI.Contracts.Bugs;
using Microsoft.AspNetCore.Authorization;

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

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> CreateBug(CreateBugRequest request)
	{
		var reportedBy = (User)HttpContext.Items["User"]!;

		var bug = new Bug
		{
			ID = Guid.NewGuid(),
			Title = request.Title,
			Description = request.Description,
			CreatedAt = DateTime.UtcNow,
			LastUpdatedAt = null,
			ProjectID = request.ProjectID,
			UserID = reportedBy.ID,
			LogFile = request.LogFile
		};

		await _bugService.CreateBug(bug);

		return CreatedAtAction(actionName: nameof(GetBugByID), routeValues: new { id = bug.ID }, value: _bugService.MapBugResponse(bug));
	}

	[HttpPatch("{id:Guid}")]
	[Authorize]
	public async Task<IActionResult> UpsertBug(Guid id, UpsertBugRequest request)
	{
		var bug = await _bugService.GetBugByID(id);
		var user = (User)HttpContext.Items["User"]!;

		// if the user does have the role of 'user', then it must be the same
		// user that reported this bug in order to update content of it
		bool doesUserHaveTheRoleUser = user.Role is "user";
		bool didThisUserReportThisBug = user.ID == bug.UserID;

		if (doesUserHaveTheRoleUser && !didThisUserReportThisBug)
			throw new ApiException(403, "Only the reporter of this bug is permitted to update.");

		// update content of the bug report
		bug.Title = request.Title ?? bug.Title;
		bug.Description = request.Description ?? bug.Description;
		bug.LogFile = request.LogFile ?? bug.LogFile;
		bug.LastUpdatedAt = DateTime.UtcNow;

		await _bugService.UpsertBug(bug);
		return SendResponse(bug);
	}

	[HttpDelete("{id:Guid}")]
	[Authorize(Roles = "leader,admin")]
	public async Task<IActionResult> DeleteBug(Guid id)
	{
		await _bugService.DeleteBug(id);
		return SendResponse(null, 204);
	}
}
