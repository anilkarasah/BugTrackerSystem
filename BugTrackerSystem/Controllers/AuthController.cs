using BugTrackerAPI.Common.Authentication;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Controllers;

public class AuthController : ApiController
{
	private readonly IAuthService _authService;
	private readonly IJwtUtils _jwtGenerator;
	public AuthController(IAuthService authService, IJwtUtils jwtGenerator)
	{
		_authService = authService;
		_jwtGenerator = jwtGenerator;
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

		return SendResponse(
			new AuthenticationResponse(
				user.ID,
				user.Name,
				user.Email,
				user.Role,
				null),
			201);
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginUserRequest request)
	{
		var loggedInUser = await _authService.Login(request.Email, request.Password);
		var token = _jwtGenerator.GenerateToken(loggedInUser.ID, loggedInUser.Name, loggedInUser.Email, loggedInUser.Role);

		return SendResponse(new AuthenticationResponse(
				loggedInUser.ID,
				loggedInUser.Name,
				loggedInUser.Email,
				loggedInUser.Role,
				token
			));
	}
}
