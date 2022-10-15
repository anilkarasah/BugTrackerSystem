using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

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
		await _context.AddAsync(request);
		_context.Entry(request).State = EntityState.Added;

		await Save();
	}
	
	private IQueryable<ProjectResponse> GetProjectResponseFromDataContext()
	{
		return _context.Projects
			.AsSplitQuery()
			.Include(p => p.Bugs)
			.Include(p => p.Contibutors)
			.Include(p => p.Leader)
			.Select(project => new ProjectResponse(
				project.ID,
				project.Name,
				project.Leader.Name,
				project.Contibutors.Count(),
				project.Contibutors
					.Select(c => new ContributorData(c.UserID, c.User.Name))
					.ToArray(),
				project.Bugs.Count(),
				project.Bugs
					.Select(b => new BugReportData(b.ID, b.Title))
					.ToArray()
			));
	}

	public async Task<List<ProjectResponse>> GetAllProjects()
	{
		var projects = await GetProjectResponseFromDataContext().ToListAsync();

		if (projects is null || !projects.Any())
			throw new ApiException(404, "No projects found.");

		return projects;
	}

	public async Task<object[]> GetMinimalProjectData()
	{
		var projectsData = await _context.Projects
			.Select(p => new { p.ID, p.Name })
			.ToArrayAsync();

		return projectsData;
	}

	public async Task<ProjectResponse> GetProjectByID(Guid projectID)
	{
		var project = await GetProjectResponseFromDataContext().FirstOrDefaultAsync(p => p.ID == projectID);

		if (project is null)
			throw new ApiException(404, $"No project found with ID: {projectID}.");

		return project;
	}

	public async Task<Project> UpsertProject(Guid projectId, UpsertProjectRequest request)
	{
		var project = await _context.Projects
			.FirstOrDefaultAsync(p => p.ID == projectId);

		if (project is null)
			throw new ApiException(404, "No project found.");

		project.Name = request.Name ?? project.Name;

		_context.Entry(project).State = EntityState.Modified;
		await Save();

		return project;
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
			UserID = contributorID,
		};

		await _context.AddAsync(newProjectUser);
		await Save();

		return newProjectUser;
	}

	public async Task RemoveContributor(Guid projectID, Guid contributorID)
	{
		var project = await GetProjectByID(projectID);

		bool doesUserExist = await _context.Users
			.AnyAsync(u => u.ID == contributorID);
		if (!doesUserExist)
			throw new ApiException(404, $"No user found with ID {contributorID}");

		var contributor = project.ContributorNamesList
			.FirstOrDefault(c => c.ID == contributorID);
		if (contributor is null)
			throw new ApiException(400, "User is not a contributor of this project.");

		var projectContributorRelationObject = await _context.ProjectUsers
			.FirstOrDefaultAsync(pu => pu.UserID == contributorID && pu.ProjectID == projectID);
		if (projectContributorRelationObject is null)
			throw new ApiException();

		_context.ProjectUsers.Remove(projectContributorRelationObject);
		await Save();
	}

	public async Task<List<User>> GetContributorsList(Guid projectID)
	{
		var contributorsList = await _context.ProjectUsers
			.Where(pu => pu.ProjectID == projectID)
			.ToListAsync();
		if (!contributorsList.Any())
			throw new ApiException(404, "No contributor found.");

		List<User> response = new();
		foreach (var u in contributorsList)
			response.Add(await _context.Users.FindAsync(u.UserID));

		return response;
	}

	public async Task<List<Bug>> GetBugReportsList(Guid projectID)
	{
		var bugsList = await _context.Bugs
			.Where(b => b.ProjectID == projectID)
			.ToListAsync();
		if (!bugsList.Any())
			throw new ApiException(404, "No bug reports found.");

		return bugsList;
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
