using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;
using Microsoft.EntityFrameworkCore;

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

	public async Task<ProjectData[]> GetMinimalProjectData()
	{
		var projectsData = await _context.Projects.Select(p => new ProjectData(p.ID, p.Name)).ToArrayAsync();

		return projectsData;
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

	public async Task RemoveContributor(Guid projectID, Guid contributorID)
	{
		var project = await GetProjectByID(projectID);

		bool doesUserExist = await _context.Users.AnyAsync(u => u.ID == contributorID);
		if (!doesUserExist)
			throw new ApiException(404, $"No user found with ID {contributorID}");

		var contributor = project.Contibutors.FirstOrDefault(c => c.UserID == contributorID);
		if (contributor is null)
			throw new ApiException(400, "User is not a contributor of this project.");

		var projectContributorRelationObject = await _context.ProjectUsers.FirstOrDefaultAsync(pu => pu.UserID == contributorID && pu.ProjectID == projectID);
		if (projectContributorRelationObject is null)
			throw new ApiException();

		_context.ProjectUsers.Remove(projectContributorRelationObject);
		await Save();
	}

	public async Task<List<User>> GetContributorsList(Guid projectID)
	{
		var contributorsList = await _context.ProjectUsers.Where(pu => pu.ProjectID == projectID).ToListAsync();
		if (!contributorsList.Any())
			throw new ApiException(404, "No contributor found.");

		List<User> response = new();
		foreach (var u in contributorsList)
			response.Add(await _context.Users.FindAsync(u.UserID));

		return response;
	}

	public async Task<List<Bug>> GetBugReportsList(Guid projectID)
	{
		var bugsList = await _context.Bugs.Where(b => b.ProjectID == projectID).ToListAsync();
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

	public async Task<ProjectResponse> MapProjectResponse(Project project)
	{
		var contributorsList = await _context.ProjectUsers
								.Where(u => u.ProjectID == project.ID)
								.Include(pu => pu.User)
								.Select(pu => new ContributorData(pu.UserID, pu.User.Name))
								.ToArrayAsync();

		var bugReportsList = await _context.Bugs
								.Where(b => b.ProjectID == project.ID)
								.Select(b => new BugReportData(b.ID, b.Title))
								.ToArrayAsync();

		var leader = await _context.Users
								.Select(u => new {u.ID, u.Name})
								.FirstOrDefaultAsync(u => u.ID == project.LeaderID);

		return new ProjectResponse(
			project.ID,
			project.Name,
			leader?.Name,
			contributorsList.Length,
			contributorsList,
			bugReportsList.Length,
			bugReportsList);
	}
}
