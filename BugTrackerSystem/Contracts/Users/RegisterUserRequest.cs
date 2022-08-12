using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Users;

public record RegisterUserRequest(
	[Required] string Name,
	[Required, EmailAddress(ErrorMessage = "Please provide a valid email address.")] string Email,
	[Required, MinLength(8, ErrorMessage = "Password must at least contain 8 characters.")] string Password);
