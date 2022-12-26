using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Contracts.Projects;

public record ProjectResponse(
	Guid ID,
	string Name,
	string LeaderName,
	ContributorData[] ContributorNamesList,
	BugReportData[] BugReportsList);

