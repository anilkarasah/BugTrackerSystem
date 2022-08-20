using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerAPI.Models;

public class Bug
{
	[Key]
	public int ID { get; set; }

	[Required]
	[StringLength(50, MinimumLength = 5, ErrorMessage = "Title must be between 5 to 30 characters long.")]
	public string Title { get; set; }

	[Required]
	public string Description { get; set; }

	public DateTime CreatedAt { get; set; }
	public DateTime LastUpdatedAt { get; set; }
	public string Status { get; set; }

	[Required]
	public Guid UserID { get; set; }
	public virtual User User { get; set; }

	[Required]
	public Guid ProjectID { get; set; }
	public virtual Project Project { get; set; }
}
