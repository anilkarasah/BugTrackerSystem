namespace BugTrackerAPI.Contracts.Bugs;

public record BugResponse(
	Guid ID,
	string Title,
	string Description,
	DateTime CreatedAt,
	Project Project,
	string? LogFile);
