namespace BugTrackerAPI.Common.Authentication.Jwt;

public interface IJwtUtils
{
	string GenerateToken(Guid ID, string Name, string Role);
	Guid? ValidateToken(string token);
}
