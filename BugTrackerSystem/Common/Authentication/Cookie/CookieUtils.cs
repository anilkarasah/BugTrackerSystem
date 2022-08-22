using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace BugTrackerAPI.Common.Authentication.Cookie;

public class CookieUtils : ICookieUtils
{
	private readonly IUserService _userService;
	public CookieUtils(IUserService userService)
	{
		_userService = userService;
	}

	public async Task SetCookie(HttpContext context, User user)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.Name),
			new Claim(ClaimTypes.Email,user.Email),
			new Claim(ClaimTypes.Role, user.Role),
			new Claim("UserID", user.ID.ToString())
		};

		var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		var authProperties = new AuthenticationProperties();

		await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
	}

	public async Task<User> GetUserFromCookie(IEnumerable<Claim> claims)
	{
		var userIdClaim = claims.First(c => c.Type == "UserID");
		if (userIdClaim is null)
			throw new ApiException(500, "Cookie error.");

		var loggedInUser = await _userService.GetUserByID(Guid.Parse(userIdClaim.Value));

		return loggedInUser;
	}
}
