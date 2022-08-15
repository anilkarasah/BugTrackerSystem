using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Bugs;

public record CreateBugRequest(
	[Required] string Title,
	[Required] string Description,
	[Required] Guid ProjectID);
