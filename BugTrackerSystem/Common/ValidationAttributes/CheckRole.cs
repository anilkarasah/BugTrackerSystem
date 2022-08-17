using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Common.ValidationAttributes;

public sealed class CheckRole : ValidationAttribute
{
	private static readonly string[] _acceptedRoles =
	{
		"user",
		"leader",
		"admin"
	};

	public static bool IsRoleValid(string role)
	{
		foreach (var r in _acceptedRoles)
		{
			if (r.ToLower() == role.ToLower())
				return true;
		}

		return false;
	}

	protected override ValidationResult? IsValid(object? roleInput, ValidationContext validationContext)
	{
		if (IsRoleValid((string)roleInput!))
			return ValidationResult.Success;

		return new ValidationResult($"'{roleInput}' is not a valid role.");
	}
}

