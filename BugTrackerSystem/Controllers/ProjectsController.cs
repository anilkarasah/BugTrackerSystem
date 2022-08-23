using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerAPI.Controllers;

public class ProjectsController : ApiController
{
	private readonly IProjectService _projectService;
	private readonly IBugService _bugService;
	private readonly IUserService _userService;
	private readonly IAuthService _authService;
	public ProjectsController(IProjectService projectService, IBugService bugService, IUserService userService, IAuthService authService)
	{
		_projectService = projectService;
		_bugService = bugService;
		_userService = userService;
		_authService = authService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllProject()
	{
		var projectsList = await _projectService.GetAllProjects();

		List<ProjectResponse> projectsListResponse = new();
		foreach (var p in projectsList)
			projectsListResponse.Add(await _projectService.MapProjectResponse(p));

		return SendResponse(projectsListResponse);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetProjectByID(Guid id)
	{
		var project = await _projectService.GetProjectByID(id);

		return SendResponse(await _projectService.MapProjectResponse(project));
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> CreateProject(CreateProjectRequest request)
	{
		//var Leader = await _authService.GetAuthenticatedUser();

		var project = new Project
		{
			ID = Guid.NewGuid(),
			Name = request.Name,
			//LeaderID = Leader.ID
		};

		await _projectService.CreateProject(project);

		return CreatedAtAction(
			actionName: nameof(GetProjectByID), 
			routeValues: new { id = project.ID }, 
			value: await _projectService.MapProjectResponse(project));
	}

	[HttpPatch("{id:Guid}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> UpsertProject(Guid id, UpsertProjectRequest request)
	{
		var project = await _projectService.GetProjectByID(id);

		project.Name = request.Name ?? project.Name;

		await _projectService.UpsertProject(project);
		return SendResponse(await _projectService.MapProjectResponse(project));
	}

	[HttpDelete("{id:Guid}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> DeleteProject(Guid id)
	{
		await _projectService.DeleteProject(id);
		return SendResponse(null, 204);
	}

	[HttpPost("addContributor")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> AddContributorToProject(ProjectUserRequest request)
	{
		var relation = await _projectService.AddContributor(request.projectID, request.contributorID);

		return SendResponse(new
		{
			message = $"User '{relation.User.Name}' is now added as a contributor to '{relation.Project.Name}'",
			project = await _projectService.MapProjectResponse(relation.Project)
		}, 201);
	}

	[HttpDelete("removeContributor")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> RemoveContributorFromProject(ProjectUserRequest request)
	{
		await _projectService.RemoveContributor(request.projectID, request.contributorID);
		return SendResponse(null, 204);
	}

	[HttpGet("{projectID:Guid}/contributors")]
	public async Task<IActionResult> GetListOfContributors(Guid projectID)
	{
		var contributorsList = await _projectService.GetContributorsList(projectID);

		List<UserResponse> response = new();
		foreach (var u in contributorsList)
			response.Add(await _userService.MapUserResponse(u));

		return SendResponse(response);
	}

	[HttpGet("{projectID:Guid}/bugs")]
	public async Task<IActionResult> GetListOfBugReports(Guid projectID)
	{
		var bugsList = await _projectService.GetBugReportsList(projectID);

		List<BugResponse> response = new();
		foreach (var b in bugsList)
			response.Add(await _bugService.MapBugResponse(b));

		return SendResponse(response);
	}
}
