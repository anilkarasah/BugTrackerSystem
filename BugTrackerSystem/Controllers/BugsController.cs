using BugTrackerAPI.Common.ValidationAttributes;
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

		List<BugResponse> bugsListResponse = new();
		foreach (var b in allBugs)
			bugsListResponse.Add(await _bugService.MapBugResponse(b));

		return SendResponse(bugsListResponse);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetBugByID(int id)
	{
		var bug = await _bugService.GetBugByID(id);
		return SendResponse(await _bugService.MapBugResponse(bug));
	}

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> CreateBug(CreateBugRequest request)
	{
		var reportedBy = (User)HttpContext.Items["User"]!;
		var currentTime = DateTime.UtcNow;

		var bug = new Bug
		{
			Title = request.Title,
			Description = request.Description,
			CreatedAt = currentTime,
			LastUpdatedAt = currentTime,
			ProjectID = request.ProjectID,
			UserID = reportedBy.ID,
			Status = "Listed"
		};

		await _bugService.CreateBug(bug);

		return CreatedAtAction(actionName: nameof(GetBugByID), routeValues: new { id = bug.ID }, value: await _bugService.MapBugResponse(bug));
	}

	[HttpPatch("{id:int}")]
	[Authorize]
	public async Task<IActionResult> UpsertBug(int id, UpsertBugRequest request)
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
		bug.Title = request.Title != null ? request.Title : bug.Title;
		bug.Description = request.Description != null ? request.Description : bug.Description;
		bug.LastUpdatedAt = DateTime.UtcNow;

		if (request.Status is not null)
		{
			if (CheckBugStatus.IsStatusValid(request.Status))
				bug.Status = request.Status;
			else
				throw new ApiException(400, $"'{request.Status}' is not a valid status.");
		}

		await _bugService.UpsertBug(bug);
		return SendResponse(await _bugService.MapBugResponse(bug));
	}

	[HttpDelete("{id:int}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> DeleteBug(int id)
	{
		await _bugService.DeleteBug(id);
		return SendResponse(null, 204);
	}
}
