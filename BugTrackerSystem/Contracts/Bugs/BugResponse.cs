namespace BugTrackerAPI.Contracts.Bugs;

public record BugResponse(
	int ID,
	string Title,
	string Description,
	string Status,
	Guid ReporterID,
	string ReporterName,
	Guid ProjectID,
	string ProjectName,
	DateTime CreatedAt,
	DateTime LastUpdatedAt);
