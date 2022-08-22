using BugTrackerAPI.Common.Authentication.Cookie;
using BugTrackerAPI.Common.ValidationAttributes;
using BugTrackerAPI.Contracts.Bugs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BugTrackerAPI.Controllers;

public class BugsController : ApiController
{
	private readonly IBugService _bugService;
	private readonly ICookieUtils _cookieUtils;
	public BugsController(IBugService bugService, ICookieUtils cookieUtils)
	{
		_bugService = bugService;
		_cookieUtils = cookieUtils;
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
		var reporter = await _cookieUtils.GetUserFromCookie(User.Claims);
		var currentTime = DateTime.UtcNow;

		var bug = new Bug
		{
			Title = request.Title,
			Description = request.Description,
			CreatedAt = currentTime,
			LastUpdatedAt = currentTime,
			ProjectID = request.ProjectID,
			UserID = reporter.ID,
			Status = "Listed"
		};

		await _bugService.CreateBug(bug);
		var response = await _bugService.MapBugResponse(bug);

		return CreatedAtAction(	actionName: nameof(GetBugByID), 
								routeValues: new { id = bug.ID }, 
								value: response	);
	}

	[HttpPatch("{id:int}")]
	[Authorize]
	public async Task<IActionResult> UpsertBug(int id, UpsertBugRequest request)
	{
		var bug = await _bugService.GetBugByID(id);
		var user = await _cookieUtils.GetUserFromCookie(User.Claims);

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
