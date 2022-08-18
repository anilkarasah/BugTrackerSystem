namespace BugTrackerAPI.Contracts.Projects;

public record CreateProjectUserRequest(
	Guid contributorID,
	Guid projectID
	);
