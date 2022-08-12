namespace BugTrackerAPI.Models;

public class ProjectUser
{
	public Guid ProjectID { get; set; }
	public Project Project { get; set; }

	public Guid UserID { get; set; }
	public User User { get; set; }
}
