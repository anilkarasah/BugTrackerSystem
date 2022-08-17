using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;
using BugTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerAPI.Controllers;

public class ProjectsController : ApiController
{
	private readonly IProjectService _projectService;
	private readonly IBugService _bugService;
	public ProjectsController(IProjectService projectService, IBugService bugService)
	{
		_projectService = projectService;
		_bugService = bugService;
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
		var supervisor = (User)HttpContext.Items["User"]!;

		var project = new Project
		{
			ID = Guid.NewGuid(),
			Name = request.Name,
			LeaderID = supervisor.ID
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

	[HttpPost("{projectID:Guid}/contributor/{contributorID:Guid}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> AddContributorToProject(Guid projectID, Guid contributorID)
	{
		var relation = await _projectService.AddContributor(projectID, contributorID);

		return SendResponse(new
		{
			message = $"User '{relation.User.Name}' is now added as a contributor to '{relation.Project.Name}'",
			project = await _projectService.MapProjectResponse(relation.Project)
		}, 201);
	}

	[HttpDelete("{projectID:Guid}/contributor/{contributorID:Guid}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> RemoveContributorFromProject(Guid projectID, Guid contributorID)
	{
		await _projectService.RemoveContributor(projectID, contributorID);
		return SendResponse(null, 204);
	}

	[HttpGet("{projectID:Guid}/contributors")]
	[Authorize]
	public async Task<IActionResult> GetListOfContributors(Guid projectID)
	{
		var contributorsList = await _projectService.GetContributorsList(projectID);

		List<UserResponse> response = new();
		foreach (var u in contributorsList)
			response.Add(new UserResponse(u.ID, u.Name, u.Email, u.Role, u.ProjectsList.Count()));

		return SendResponse(response);
	}

	[HttpGet("{projectID:Guid}/bugs")]
	[Authorize]
	public async Task<IActionResult> GetListOfBugReports(Guid projectID)
	{
		var bugsList = await _projectService.GetBugReportsList(projectID);

		List<BugResponse> response = new();
		foreach (var b in bugsList)
			response.Add(await _bugService.MapBugResponse(b));

		return SendResponse(response);
	}
}
