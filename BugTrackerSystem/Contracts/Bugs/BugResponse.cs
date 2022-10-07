using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Contracts.Bugs;

public record BugResponse(
	int ID,
	string Title,
	string Description,
	string Status,
	ContributorData Reporter,
	ProjectData Project,
	DateTime CreatedAt,
	DateTime LastUpdatedAt);
