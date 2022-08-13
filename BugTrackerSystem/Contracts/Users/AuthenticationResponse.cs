namespace BugTrackerAPI.Contracts.Users;

public record AuthenticationResponse(
	Guid ID,
	string Name,
	string Email,
	string Role,
	string Token
	);
