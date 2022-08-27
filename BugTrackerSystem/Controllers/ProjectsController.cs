using BugTrackerAPI.Common.Mapper;
using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerAPI.Controllers;

public class ProjectsController : ApiController
{
	private readonly IProjectService _projectService;
	private readonly IAuthService _authService;
	private readonly IMapperUtils _mapperUtils;
	public ProjectsController(
		IProjectService projectService,
		IAuthService authService,
		IMapperUtils mapperUtils)
	{
		_projectService = projectService;
		_authService = authService;
		_mapperUtils = mapperUtils;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllProject()
	{
		var projectsList = await _projectService.GetAllProjects();

		List<ProjectResponse> projectsListResponse = new();
		foreach (var p in projectsList)
			projectsListResponse.Add(await _mapperUtils.MapProjectResponse(p));

		return SendResponse(projectsListResponse);
	}

	[HttpGet("mini")]
	public async Task<IActionResult> GetMinimalProjectData()
	{
		var minimalProjectsData = await _projectService.GetMinimalProjectData();

		return SendResponse(minimalProjectsData);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetProjectByID(Guid id)
	{
		var project = await _projectService.GetProjectByID(id);

		return SendResponse(await _mapperUtils.MapProjectResponse(project));
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> CreateProject(CreateProjectRequest request)
	{
		var Leader = await _authService.GetAuthenticatedUser(HttpContext);

		var project = new Project
		{
			ID = Guid.NewGuid(),
			Name = request.Name,
			LeaderID = Leader.ID
		};

		await _projectService.CreateProject(project);

		return CreatedAtAction(
			actionName: nameof(GetProjectByID), 
			routeValues: new { id = project.ID }, 
			value: await _mapperUtils.MapProjectResponse(project));
	}

	[HttpPatch("{id:Guid}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> UpsertProject(Guid id, UpsertProjectRequest request)
	{
		var project = await _projectService.GetProjectByID(id);

		project.Name = request.Name ?? project.Name;

		await _projectService.UpsertProject(project);
		return SendResponse(await _mapperUtils.MapProjectResponse(project));
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

		var response = await _mapperUtils.MapProjectResponse(relation.Project);
		return SendResponse(response, 201);
	}

	[HttpDelete("{projectID:Guid}/contributor/{contributorID:Guid}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> RemoveContributorFromProject(Guid projectID, Guid contributorID)
	{
		await _projectService.RemoveContributor(projectID, contributorID);
		return SendResponse(null, 204);
	}

	[HttpGet("{projectID:Guid}/contributors")]
	public async Task<IActionResult> GetListOfContributors(Guid projectID)
	{
		var contributorsList = await _projectService.GetContributorsList(projectID);

		List<UserResponse> response = new();
		foreach (var u in contributorsList)
			response.Add(await _mapperUtils.MapUserResponse(u));

		return SendResponse(response);
	}

	[HttpGet("{projectID:Guid}/bugs")]
	public async Task<IActionResult> GetListOfBugReports(Guid projectID)
	{
		var bugsList = await _projectService.GetBugReportsList(projectID);

		List<BugResponse> response = new();
		foreach (var b in bugsList)
			response.Add(await _mapperUtils.MapBugResponse(b));

		return SendResponse(response);
	}
	
	[HttpGet("bugs")]
	public async Task<IActionResult> GetProjectsListForBugReport() {
		var projectsList = await _projectService.GetAllProjects();
		var responseList = new List<ProjectData>();
		
		foreach (var p in projectsList)
			responseList.Add(new (p.ID, p.Name));
		
		return SendResponse(responseList);
	}
}
