namespace BugTrackerAPI.Contracts.Projects;

public record ProjectResponse(
	Guid ID,
	string Name,
	string SupervisorName,
	int NumberOfContributors,
	object[] ContributorNamesList,
	int NumberOfBugReports,
	object[] BugReportsList);
