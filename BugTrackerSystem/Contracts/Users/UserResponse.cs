using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;

namespace BugTrackerAPI.Contracts.Users;

public record UserResponse(
	Guid ID,
	string Name,
	string Email,
	string Role,
	IEnumerable<ProjectData>? ContributedProjects,
	IEnumerable<BugReportData>? BugReports);
