using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Models;

public class User
{
	public Guid ID { get; set; }

	[Required(ErrorMessage = "Name is required.")]
	[StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 to 50 characters long.")]
	public string Name { get; set; }

	[Required(ErrorMessage = "Email is required.")]
	[EmailAddress]
	public string Email { get; set; }

	[StringLength(60, MinimumLength = 8, ErrorMessage = "Password must be between 8 to 60 characters long.")]
	public string Password { get; set; }

	// Roles: user < contributor < leader < admin
	[StringLength(12)]
	public string Role { get; set; }

	public virtual ICollection<ProjectUser> ProjectsList { get; set; }
}
