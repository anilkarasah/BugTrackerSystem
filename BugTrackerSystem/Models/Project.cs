using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BugTrackerAPI.Models;

public class Project
{
	public Guid ID { get; set; }

	[Required(ErrorMessage = "Name of the project is required.")]
	[StringLength(50, MinimumLength = 5, ErrorMessage = "Name must be between 5 to 50 characters long.")]
	public string Name { get; set; }
	
	public Guid LeaderID { get; set; }
	public virtual User Leader { get; set; }

	public virtual ICollection<ProjectUser> Contibutors { get; set; }

	[JsonIgnore]
	public virtual ICollection<Bug> Bugs { get; set; }
}
