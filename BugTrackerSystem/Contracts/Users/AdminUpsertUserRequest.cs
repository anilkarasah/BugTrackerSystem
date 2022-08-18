using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Users;

public record AdminUpsertUserRequest(
	string? Name,
	string? Email,
	[StringLength(20, MinimumLength = 8, ErrorMessage = "New password must be between 8 to 20 characters.")] string? NewPassword,
	string? Role);
