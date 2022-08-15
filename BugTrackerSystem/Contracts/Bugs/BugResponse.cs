namespace BugTrackerAPI.Contracts.Bugs;

public record BugResponse(
	int ID,
	string Title,
	string Description,
	DateTime CreatedAt,
	Project Project);
