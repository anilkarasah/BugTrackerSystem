﻿using BugTrackerAPI.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using BugTrackerAPI.Common.Authentication.Jwt;
using BugTrackerAPI.Common.Mapper;

namespace BugTrackerAPI.Controllers;

public class AuthController : ApiController
{
	private readonly CookieOptions _cookieOptions = new CookieOptions
	{
		SameSite = SameSiteMode.None,
		Secure = true,
		Expires = DateTimeOffset.Now.AddDays(1)
	};

	private readonly IAuthService _authService;
	private readonly IJwtUtils _jwtUtils;
	public AuthController(IAuthService authService, IJwtUtils jwtUtils)
	{
		_authService = authService;
		_jwtUtils = jwtUtils;
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

		return CreatedAtAction(actionName: nameof(Login), value: MapperUtils.MapAuthenticationResponse(user));
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginUserRequest request)
	{
		var loggedInUser = await _authService.Login(request.Email, request.Password);
		var token = _jwtUtils.GenerateToken(loggedInUser.ID, loggedInUser.Name, loggedInUser.Role);

		var response = new
		{
			token,
			user = MapperUtils.MapAuthenticationResponse(loggedInUser)
		};

		HttpContext.Response.Cookies.Append("jwt", token, _cookieOptions);
		return SendResponse(response);
	}

	[Authorize]
	[HttpPost("logout")]
	public IActionResult Logout()
	{
		HttpContext.Response.Cookies.Delete("jwt", _cookieOptions);

		return SendResponse(new { Message = "OK" }, 200);
	}
}
