namespace BugTrackerAPI.Contracts.Users;

public record UpsertUserRequest(
	string? Name,
	string? Email,
	string? CurrentPassword,
	string? NewPassword);
