namespace BugTrackerAPI.Contracts.Users;

public record UserResponse(
	Guid ID,
	string Name,
	string Email,
	string Role,
	int NumberOfContributedProjects,
	object[] ContributedProjects,
	int NumberOfBugReports,
	object[] BugReports);
