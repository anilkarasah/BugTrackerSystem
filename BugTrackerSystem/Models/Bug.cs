using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerAPI.Models;

public class Bug
{
	[Key]
	public Guid ID { get; set; }

	[Required]
	[StringLength(30, MinimumLength = 5, ErrorMessage = "Title must be between 5 to 30 characters long.")]
	public string Title { get; set; }

	[Required]
	[StringLength(500, MinimumLength = 20, ErrorMessage = "Password must be between 20 to 500 characters long.")]
	public string Description { get; set; }
	public DateTime CreatedAt { get; set; }

	[StringLength(100)]
	public string? LogFile { get; set; }

	public virtual Project Project { get; set; }
	public Guid ProjectID { get; set; }
}
