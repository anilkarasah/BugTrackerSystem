namespace BugTrackerAPI.Common.Authentication.Jwt;

public interface IJwtUtils
{
	public string GenerateToken(Guid userID, string name, string email, string role);
	public Guid? ValidateToken(string token);
}
