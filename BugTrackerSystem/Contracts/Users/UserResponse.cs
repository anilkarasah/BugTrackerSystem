namespace BugTrackerAPI.Contracts.Users;

public record UserResponse(
	Guid ID,
	string Name,
	string Email,
	string Roles,
	ICollection<ProjectUser> ProjectsList);
