namespace BugTrackerAPI.Services;

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
		var projects = await _context.Projects.Include(p => p.Contibutors).ToListAsync();

		if (projects is null || !projects.Any())
			throw new ApiException(404, "No projects found.");

		return projects;
	}

	public async Task<Project> GetProjectByID(Guid projectID)
	{
		var project = await _context.Projects.Include(p => p.Contibutors).SingleAsync(p => p.ID == projectID);

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

	public async Task<ProjectUser> AddContributor(Guid projectID, Guid contributorID)
	{
		var user = await _context.Users.FindAsync(contributorID);
		if (user is null)
			throw new ApiException(404, $"No user found with ID: {contributorID}");

		var project = await GetProjectByID(projectID);

		var newProjectUser = new ProjectUser
		{
			ProjectID = projectID,
			Project = project,
			UserID = contributorID,
			User = user
		};

		await _context.AddAsync(newProjectUser);
		await Save();

		return newProjectUser;
	}

	public async Task Save()
	{
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (Exception e)
		{
			throw new ApiException(500, e.Message);
		}
	}
}
