using BCryptNet = BCrypt.Net.BCrypt;

namespace BugTrackerAPI.Common.Authentication.Hash;

public class HashUtils : IHashUtils
{
	public bool VerifyCurrentPassword(string candidatePassword, string currentPassword)
	{
		return BCryptNet.Verify(text: candidatePassword, hash: currentPassword);
	}

	public string HashPassword(string newPassword)
	{
		// Hash password by 12 iterations
		return BCryptNet.HashPassword(inputKey: newPassword, workFactor: 12);
	}
}
