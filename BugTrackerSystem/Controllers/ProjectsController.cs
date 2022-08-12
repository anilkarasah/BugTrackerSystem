using BugTrackerAPI.Contracts.Projects;

namespace BugTrackerAPI.Controllers;

public class ProjectsController : ApiController
{
	private readonly IProjectService _projectService;
	public ProjectsController(IProjectService projectService)
	{
		_projectService = projectService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllProject()
	{
		var projectsList = await _projectService.GetAllProjects();
		return SendResponse(projectsList);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetProjectByID(Guid id)
	{
		var project = await _projectService.GetProjectByID(id);
		return SendResponse(project);
	}

	[HttpPost]
	public async Task<IActionResult> CreateProject(CreateProjectRequest request)
	{
		var project = new Project
		{
			ID = Guid.NewGuid(),
			Name = request.Name
		};

		await _projectService.CreateProject(project);

		return CreatedAtAction(actionName: nameof(GetProjectByID), routeValues: new { id = project.ID }, value: project);
	}

	[HttpPatch("{id:Guid}")]
	public async Task<IActionResult> UpsertProject(Guid id, UpsertProjectRequest request)
	{
		var project = await _projectService.GetProjectByID(id);

		project.Name = request.Name ?? project.Name;

		await _projectService.UpsertProject(project);
		return SendResponse(project);
	}

	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> DeleteProject(Guid id)
	{
		await _projectService.DeleteProject(id);
		return SendResponse(null, 204);
	}
}
