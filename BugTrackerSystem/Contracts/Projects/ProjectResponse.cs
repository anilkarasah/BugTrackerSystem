namespace BugTrackerAPI.Contracts.Projects;

public record ProjectResponse(
	Guid ID,
	string Name,
	string LeaderName,
	int NumberOfContributors,
	object[] ContributorNamesList,
	int NumberOfBugReports,
	object[] BugReportsList);
