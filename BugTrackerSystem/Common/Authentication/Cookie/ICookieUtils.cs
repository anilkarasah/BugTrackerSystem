using System.Security.Claims;

namespace BugTrackerAPI.Common.Authentication.Cookie;

public interface ICookieUtils
{
	Task SetCookie(HttpContext context, User user);
	Task<User> GetUserFromCookie(IEnumerable<Claim> claims);
}
