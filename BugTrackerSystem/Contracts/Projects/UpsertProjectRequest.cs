using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Projects;

public record UpsertProjectRequest(
	[MinLength(5), MaxLength(30)] string Name);
