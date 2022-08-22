using BugTrackerAPI.Contracts.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using BugTrackerAPI.Common.Authentication.Cookie;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerAPI.Controllers;

public class AuthController : ApiController
{
	private readonly IAuthService _authService;
	private readonly ICookieUtils _cookieUtils;
	public AuthController(IAuthService authService, ICookieUtils cookieUtils)
	{
		_authService = authService;
		_cookieUtils = cookieUtils;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterUserRequest request)
	{
		var userID = Guid.NewGuid();

		var user = new User
		{
			ID = userID,
			Name = request.Name.Trim(),
			Email = request.Email.Trim(),
			Password = request.Password.Trim(),
			Role = "user"
		};

		await _authService.Register(user);

		return CreatedAtAction(actionName: nameof(Login), value: _authService.MapAuthenticationResponse(user));
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginUserRequest request)
	{
		var loggedInUser = await _authService.Login(request.Email, request.Password);

		await _cookieUtils.SetCookie(HttpContext, loggedInUser);

		return SendResponse(_authService.MapAuthenticationResponse(loggedInUser));
	}

	[Authorize]
	[HttpPost("logout")]
	public async Task Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
	}
}
