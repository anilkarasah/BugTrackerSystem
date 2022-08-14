using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Bugs;

public record UpsertBugRequest(
	[MinLength(5), MaxLength(50)] string? Title,
	[MaxLength(500)] string? Description,
	string? LogFile,
	bool? IsFixed);
