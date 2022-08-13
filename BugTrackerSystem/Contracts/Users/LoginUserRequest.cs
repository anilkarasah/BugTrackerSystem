using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Users;

public record LoginUserRequest(
	[Required] string Email,
	[Required, MinLength(8)] string Password);

