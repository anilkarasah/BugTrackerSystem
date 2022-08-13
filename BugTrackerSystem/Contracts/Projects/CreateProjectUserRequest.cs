namespace BugTrackerAPI.Contracts.Projects;

public record CreateProjectUserRequest(
	Guid userID,
	Guid projectID
	);
