using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Contracts.Bugs;

public record CreateBugRequest(
	[Required,
	StringLength(50, MinimumLength = 5, ErrorMessage = "Title of the bug must be between 5 to 50 characters.")]
		string Title,
	[Required] string Description,
	[Required] Guid ProjectID);
