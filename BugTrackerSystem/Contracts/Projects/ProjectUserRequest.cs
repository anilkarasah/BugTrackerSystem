namespace BugTrackerAPI.Contracts.Projects;

public record ProjectUserRequest(
	Guid contributorID,
	Guid projectID
	);
