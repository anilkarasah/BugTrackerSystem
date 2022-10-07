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

	public void MapBugResponse(int bugID)
	{
		var bug = _context.Bugs
			.FirstOrDefault(b => b.ID == bugID);
			
		Console.WriteLine($"--> bugID: {bug?.ID} | bugReporter: {bug?.ProjectID} - {bug?.User.Name} | bugProject: {bug?.ProjectID} - {bug?.Project.Name}");
		
		// 	.Select(b => new {
		// 		BugID = b.ID,
		// 		b.Title,
		// 		b.Description,
		// 		b.Status,
		// 		Reporter = new ContributorData(
		// 			b.UserID,
		// 			b.User.Name
		// 		),
		// 		Project = new ProjectData(
		// 			b.ProjectID,
		// 			b.Project.Name
		// 		)
		// 	});
		
		// return bug;
		
		// var project = await _context.Projects
		// 						.Select(p => new { p.ID, p.Name })
		// 						.FirstAsync(p => p.ID == b.ProjectID);

		// var reporter = await _context.Users
		// 						.Select(u => new { u.ID, u.Name })
		// 						.FirstAsync(u => u.ID == b.UserID);

		// return new BugResponse(
		// 	b.ID,
		// 	b.Title,
		// 	b.Description,
		// 	b.Status,
		// 	b.UserID,
		// 	reporter.Name,
		// 	b.ProjectID,
		// 	project.Name,
		// 	b.CreatedAt.ToString("ddd, MMM d, yyy"),
		// 	b.LastUpdatedAt.ToString("ddd, MMM d, yyy"));
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
								.Select(u => new { u.ID, u.Name })
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
