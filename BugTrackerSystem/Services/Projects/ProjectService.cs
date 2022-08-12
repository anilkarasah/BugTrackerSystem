﻿namespace BugTrackerAPI.Services;

public class ProjectService : IProjectService
{
	private readonly DataContext _context;
	public ProjectService(DataContext context)
	{
		_context = context;
	}

	public async Task CreateProject(Project request)
	{
		try
		{
			await _context.AddAsync(request);
			_context.Entry(request).State = EntityState.Added;

			await Save();
		}
		catch (Exception e)
		{
			throw new ApiException(500, e.Message);
		}
	}

	public async Task<List<Project>> GetAllProjects()
	{
		var projects = await _context.Projects.ToListAsync();

		if (projects is null || !projects.Any())
			throw new ApiException(404, "No projects found.");

		return projects;
	}

	public async Task<Project> GetProjectByID(Guid projectID)
	{
		var project = await _context.Projects.FindAsync(projectID);

		if (project is null)
			throw new ApiException(404, $"No project found with ID: {projectID}.");

		return project;
	}

	public async Task UpsertProject(Project project)
	{
		_context.Entry(project).State = EntityState.Modified;
		await Save();
	}

	public async Task DeleteProject(Guid projectID)
	{
		var project = await _context.Projects.FindAsync(projectID);

		if (project is null)
			throw new ApiException(404, $"No project found with ID: {projectID}");

		_context.Projects.Remove(project);
		_context.Entry(project).State = EntityState.Deleted;

		await Save();
	}

	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}
}