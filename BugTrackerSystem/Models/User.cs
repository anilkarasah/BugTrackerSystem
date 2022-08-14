using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BugTrackerAPI.Models;

public class User
{
	public Guid ID { get; set; }

	[Required(ErrorMessage = "Name is required.")]
	[StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 to 50 characters long.")]
	public string Name { get; set; }

	[Required(ErrorMessage = "Email is required.")]
	[EmailAddress(ErrorMessage = "Please provide a valid email address.")]
	public string Email { get; set; }

	[JsonIgnore]
	[StringLength(60)]
	public string Password { get; set; }

	// Roles: user < leader < admin
	[StringLength(12)]
	public string Role { get; set; }

	public virtual ICollection<Bug> ReportedBugs { get; set; }
	public virtual ICollection<ProjectUser> ProjectsList { get; set; }
}
