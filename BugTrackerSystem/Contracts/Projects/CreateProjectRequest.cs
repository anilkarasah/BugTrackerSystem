using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Projects;

public record CreateProjectRequest(
	[Required, MinLength(3), MaxLength(30)] string Name);
