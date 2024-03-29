﻿using BugTrackerAPI.Common.Mapper;
using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerAPI.Controllers;

public class ProjectsController : ApiController
{
	private readonly IProjectService _projectService;
	private readonly IAuthService _authService;
	public ProjectsController(
		IProjectService projectService,
		IAuthService authService)
	{
		_projectService = projectService;
		_authService = authService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllProject()
	{
		var projectsList = await _projectService.GetAllProjects();

		return SendResponse(projectsList);
	}

	[HttpGet("mini")]
	public async Task<IActionResult> GetMinimalProjectData()
	{
		var minimalProjectsData = await _projectService.GetMinimalProjectData();

		return SendResponse(minimalProjectsData);
	}

	[HttpGet("{id:Guid}")]
	public IActionResult GetProjectByID(Guid id)
	{
		var project = _projectService.GetProjectByID(id);
		
		return SendResponse(project);
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
			value: MapperUtils.MapProjectResponse(project));
	}

	[HttpPatch("{id:Guid}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> UpsertProject(Guid id, UpsertProjectRequest request)
	{
		var updatedProject = await _projectService.UpsertProject(id, request);
		var response = MapperUtils.MapProjectResponse(updatedProject);
		return SendResponse(response);
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

		return SendResponse(new { status = "OK" }, 201);
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

		var response = MapperUtils.MapAllUserResponses(contributorsList);

		return SendResponse(response);
	}

	[HttpGet("{projectID:Guid}/bugs")]
	public async Task<IActionResult> GetListOfBugReports(Guid projectID)
	{
		var bugsList = await _projectService.GetBugReportsList(projectID);

		var response = MapperUtils.MapAllBugResponses(bugsList).ToList();

		return SendResponse(response);
	}
}
