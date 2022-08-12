using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Users;

public record LoginUserRequest(
	[Required] string Email,
	[Required] string Password);

