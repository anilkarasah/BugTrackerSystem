using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Common.Mapper;

public class MapperUtils : IMapperUtils
{
	private readonly DataContext _context;
	public MapperUtils(DataContext context)
	{
		_context = context;
	}

	public AuthenticationResponse MapAuthenticationResponse(User user)
	{
		return new AuthenticationResponse(
			user.ID,
			user.Name,
			user.Email,
			user.Role);
	}

	public BugResponse? MapBugResponse(int bugID)
	{
		var bug = _context.Bugs
			.Where(b => b.ID == bugID)
			.Include(b => b.User)
			.Include(b => b.Project)
			.Select(b => new BugResponse(
				b.ID,
				b.Title,
				b.Description,
				b.Status,
				new ContributorData(b.UserID, b.User.Name),
				new ProjectData(b.ProjectID, b.Project.Name),
				b.CreatedAt,
				b.LastUpdatedAt
			)).FirstOrDefault();
		
		return bug;
	}

	public ProjectResponse? MapProjectResponse(Guid projectID)
	{
		var project = _context.Projects
			.Where(p => p.ID == projectID)
			.Include(p => p.Bugs)
			.Include(p => p.Contibutors)
			.Select(p => new ProjectResponse(
				p.ID,
				p.Name,
				_context.Users.Where(leader => leader.ID == p.LeaderID).Select(leader => leader.Name).First(),
				p.Contibutors.Count(),
				p.Contibutors.Select(c => new ContributorData(c.UserID, c.User.Name)).ToArray(),
				p.Bugs.Count(),
				p.Bugs.Select(b => new BugReportData(b.ID, b.Title)).ToArray()
			)).FirstOrDefault();
		
		return project;
		// var contributorsList = await _context.ProjectUsers
		// 						.Where(u => u.ProjectID == project.ID)
		// 						.Include(pu => pu.User)
		// 						.Select(pu => new ContributorData(pu.UserID, pu.User.Name))
		// 						.ToArrayAsync();

		// var bugReportsList = await _context.Bugs
		// 						.Where(b => b.ProjectID == project.ID)
		// 						.Select(b => new BugReportData(b.ID, b.Title))
		// 						.ToArrayAsync();

		// var leader = await _context.Users
		// 						.Select(u => new { u.ID, u.Name })
		// 						.FirstOrDefaultAsync(u => u.ID == project.LeaderID);

		// return new ProjectResponse(
		// 	project.ID,
		// 	project.Name,
		// 	leader?.Name,
		// 	contributorsList.Length,
		// 	contributorsList,
		// 	bugReportsList.Length,
		// 	bugReportsList);
	}

	public async Task<UserResponse> MapUserResponse(User user)
	{
		var contributions = await _context.ProjectUsers
								.Include(pu => pu.Project)
								.Where(pu => pu.UserID == user.ID)
								.Select(pu => new ProjectData(pu.ProjectID, pu.Project.Name))
								.ToArrayAsync();

		var bugReports = await _context.Bugs
								.Where(b => b.UserID == user.ID)
								.Select(b => new BugReportData(b.ID, b.Title))
								.ToArrayAsync();

		return new UserResponse(
			user.ID,
			user.Name,
			user.Email,
			user.Role,
			contributions.Length,
			contributions,
			bugReports.Length,
			bugReports);
	}
}
