namespace BugTrackerAPI.Common.Authentication.Hash
{
	public interface IHashUtils
	{
		public bool VerifyCurrentPassword(string candidatePassword, string currentPassword);
		public string HashPassword(string newPassword);
	}
}
