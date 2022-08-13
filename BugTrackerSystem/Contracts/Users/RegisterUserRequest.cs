using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Users;

public record RegisterUserRequest(
	[Required] string Name,
	[Required, EmailAddress(ErrorMessage = "Please provide a valid email address.")] string Email,
	[Required, StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 to 20 characters long.")] string Password);
