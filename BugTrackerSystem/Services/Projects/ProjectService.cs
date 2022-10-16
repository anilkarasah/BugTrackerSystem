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

	public async Task<List<ProjectResponse>> GetAllProjects()
	{
		var projectsListResponse = await _context.Projects
			.AsSplitQuery()
			.Include(p => p.Bugs)
			.Include(p => p.Contibutors)
			.Include(p => p.Leader)
			.Select(project => 
				new ProjectResponse(
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
				)
			)
			.ToListAsync();

		if (projectsListResponse is null 
			|| !projectsListResponse.Any())
			throw new ApiException(404, "No projects found.");

		return projectsListResponse;
	}

	public async Task<ProjectData[]> GetMinimalProjectData()
	{
		var projectsDataResponse = await _context.Projects
			.Select(p => new ProjectData(p.ID, p.Name))
			.ToArrayAsync();

		return projectsDataResponse;
	}

	public async Task<ProjectResponse> GetProjectByID(Guid projectID)
	{
		var projectResponse = await _context.Projects
			.AsSplitQuery()
			.Include(p => p.Bugs)
			.Include(p => p.Contibutors)
			.Include(p => p.Leader)
			.Select(project => 
				new ProjectResponse(
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
				)
			)
			.SingleOrDefaultAsync(p => p.ID == projectID);

		if (projectResponse is null)
			throw new ApiException(404, $"No project found with ID: {projectID}.");

		return projectResponse;
	}

	public async Task<Project> UpsertProject(Guid projectId, UpsertProjectRequest request)
	{
		var projectToBeUpserted = await _context.Projects
			.FirstOrDefaultAsync(p => p.ID == projectId);

		if (projectToBeUpserted is null)
			throw new ApiException(404, "No project found.");

		projectToBeUpserted.Name = request.Name ?? projectToBeUpserted.Name;

		_context.Entry(projectToBeUpserted).State = EntityState.Modified;
		await Save();

		return projectToBeUpserted;
	}

	public async Task DeleteProject(Guid projectID)
	{
		var projectToBeDeleted = await _context.Projects.FindAsync(projectID);

		if (projectToBeDeleted is null)
			throw new ApiException(404, $"No project found with ID: {projectID}");

		_context.Projects.Remove(projectToBeDeleted);
		_context.Entry(projectToBeDeleted).State = EntityState.Deleted;

		await Save();
	}

	public async Task<ProjectUser> AddContributor(Guid projectID, Guid contributorID)
	{
		var contributor = await _context.Users.FindAsync(contributorID);
		if (contributor is null)
			throw new ApiException(404, $"No user found with ID: {contributorID}");

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
		var projectToRemoveTheContributor = await GetProjectByID(projectID);

		bool doesUserExist = await _context.Users
			.AnyAsync(u => u.ID == contributorID);
		if (!doesUserExist)
			throw new ApiException(404, $"No user found with ID {contributorID}");

		var contributorToRemove = projectToRemoveTheContributor.ContributorNamesList
			.FirstOrDefault(c => c.ID == contributorID);
		if (contributorToRemove is null)
			throw new ApiException(400, "User is not a contributor of this project.");

		var projectContributorRelationObject = await _context.ProjectUsers
			.FirstOrDefaultAsync(pu => pu.UserID == contributorID && pu.ProjectID == projectID);
		if (projectContributorRelationObject is null)
			throw new ApiException(500, "Something went wrong while trying to remove the contributor.");

		_context.ProjectUsers.Remove(projectContributorRelationObject);
		await Save();
	}

	public async Task<List<User>> GetContributorsList(Guid projectID)
	{
		var contributorsIDList = await _context.ProjectUsers
			.Where(pu => pu.ProjectID == projectID)
			.Select(pu => pu.UserID)
			.ToListAsync();
			
		if (contributorsIDList is null || !contributorsIDList.Any())
			throw new ApiException(404, "No contributor found.");

		var usersList = await _context.Users
			.Where(user => contributorsIDList.Contains(user.ID))
			.ToListAsync();

		return usersList;
	}

	public async Task<List<Bug>> GetBugReportsList(Guid projectID)
	{
		var bugsList = await _context.Bugs
			.Where(b => b.ProjectID == projectID)
			.ToListAsync();

		if (bugsList is null || !bugsList.Any())
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
