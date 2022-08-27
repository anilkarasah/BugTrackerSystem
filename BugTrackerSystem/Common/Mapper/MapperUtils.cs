﻿using BugTrackerAPI.Contracts.Bugs;
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

	public async Task<BugResponse> MapBugResponse(Bug b)
	{
		var project = await _context.Projects
								.Select(p => new { p.ID, p.Name })
								.FirstAsync(p => p.ID == b.ProjectID);

		var reporter = await _context.Users
								.Select(u => new { u.ID, u.Name })
								.FirstAsync(u => u.ID == b.UserID);

		return new BugResponse(
			b.ID,
			b.Title,
			b.Description,
			b.Status,
			b.ProjectID,
			reporter.Name,
			b.UserID,
			project.Name,
			b.CreatedAt.ToString("ddd, MMM d, yyy", new System.Globalization.CultureInfo("en-US")),
			b.LastUpdatedAt.ToString("ddd, MMM d, yyy", new System.Globalization.CultureInfo("en-US")));
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