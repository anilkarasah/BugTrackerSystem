using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Common.ValidationAttributes;

public sealed class CheckBugStatus : ValidationAttribute
{
	private static readonly string[] _acceptedStatusInputs =
	{
		"Listed",
		"To Do",
		"Tracking",
		"Solved"
	};

	public static bool IsStatusValid(string status)
	{
		foreach (var s in _acceptedStatusInputs)
		{
			if (s.ToLower() == status.ToLower())
				return true;
		}

		return false;
	}

	protected override ValidationResult? IsValid(object? status, ValidationContext validationContext)
	{
		if (IsStatusValid((string)status!))
			return ValidationResult.Success;

		return new ValidationResult($"'{status}' is not a valid status.");
	}
}

