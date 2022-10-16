using BugTrackerAPI.Common.Mapper;
using BugTrackerAPI.Common.ValidationAttributes;
using BugTrackerAPI.Contracts.Bugs;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerAPI.Controllers;

public class BugsController : ApiController
{
	private readonly IBugService _bugService;
	private readonly IAuthService _authService;
	public BugsController(IBugService bugService, IAuthService authService)
	{
		_bugService = bugService;
		_authService = authService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllBugs()
	{
		var bugsListResponse = await _bugService.GetBugs();

		return SendResponse(bugsListResponse);
	}

	[HttpGet("mini")]
	public async Task<IActionResult> GetMinimalBugData()
	{
		var minimalBugsData = await _bugService.GetMinimalBugData();

		return SendResponse(minimalBugsData);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetBugByID(int id)
	{
		var response = await _bugService.GetBugByID(id);
		return SendResponse(response);
	}

	[Authorize(Roles = "admin")]
	[HttpPost]
	public async Task<IActionResult> CreateBug(CreateBugRequest request)
	{
		var reporter = await _authService.GetAuthenticatedUser(HttpContext);
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
		var response = MapperUtils.MapBugResponse(bug);

		return CreatedAtAction(
			actionName: nameof(GetBugByID),
			routeValues: new { id = bug.ID },
			value: response);
	}

	[HttpPatch("{id:int}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> UpsertBug(int id, UpsertBugRequest request)
	{
		if (string.IsNullOrWhiteSpace(request.Status)
			|| !CheckBugStatus.IsStatusValid(request.Status))
		{
			throw new ApiException(400, $"'{request.Status}' is not a valid status.");
		}

		var upsertedBugResponse = await _bugService.UpsertBug(id, request);
		
		var response = MapperUtils.MapBugResponse(upsertedBugResponse);
		return SendResponse(response);
	}

	[HttpDelete("{id:int}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> DeleteBug(int id)
	{
		await _bugService.DeleteBug(id);
		return SendResponse(null, 204);
	}
}
