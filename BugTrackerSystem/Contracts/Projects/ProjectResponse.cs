namespace BugTrackerAPI.Contracts.Projects;

public record ProjectResponse(
	Guid ID,
	string Name,
	int NumberOfContributors,
	int NumberOfBugReports);
