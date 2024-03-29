﻿using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Common.Mapper;

public static class MapperUtils
{
	public static AuthenticationResponse MapAuthenticationResponse(User user)
	{
		return new AuthenticationResponse(
			user.ID,
			user.Name,
			user.Email,
			user.Role);
	}

	public static BugResponse MapBugResponse(Bug bug)
	{
		var bugResponse = new BugResponse(
			bug.ID,
			bug.Title,
			bug.Description,
			bug.Status,
			new ContributorData(bug.UserID, bug.User?.Name),
			new ProjectData(bug.ProjectID, bug.Project?.Name),
			bug.CreatedAt,
			bug.LastUpdatedAt
		);
		
		return bugResponse;
	}
	
	public static IEnumerable<BugResponse> MapAllBugResponses(List<Bug> bugsList)
	{
		foreach (var bug in bugsList) {
			var mappedBug = MapBugResponse(bug);
			yield return mappedBug;
		}
	}

	public static ProjectResponse MapProjectResponse(Project project)
	{
		var response = new ProjectResponse(
			project.ID,
			project.Name,
			project.Leader.Name,
			project.Contibutors?
				.Select(c => new ContributorData(c.UserID, c.User?.Name))
			,
			project.Bugs?
				.Select(b => new BugReportData(b.ID, b.Title))
		);
		
		return response;
	}
	
	public static IEnumerable<ProjectResponse> MapAllProjectResponses(List<Project> projectsList)
	{
		foreach (var project in projectsList)
		{
			var mappedProject = MapProjectResponse(project);
			yield return mappedProject;
		}
	}

	public static UserResponse MapUserResponse(User user)
	{
		var response = new UserResponse(
			user.ID,
			user.Name,
			user.Email,
			user.Role,
			user.ProjectsList?
				.Select(p => new ProjectData(p.ProjectID, p.Project?.Name)),
			user.ReportedBugs?
				.Select(b => new BugReportData(b.ID, b.Title))
		);
		
		return response;
	}
	
	public static IEnumerable<UserResponse> MapAllUserResponses(List<User> usersList)
	{
		foreach (var user in usersList)
		{
			var mappedUser = MapUserResponse(user);
			yield return mappedUser;
		}
	}
}
