using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Controllers;

public class AuthController : ApiController
{
	private readonly IAuthService _authService;
	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterUserRequest request)
	{
		var user = new User
		{
			ID = Guid.NewGuid(),
			Name = request.Name.Trim(),
			Email = request.Email.Trim(),
			Password = request.Password.Trim(),
			Role = "user"
		};

		await _authService.Register(user);

		return SendResponse(new { user.ID, user.Name, user.Email }, 201);
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginUserRequest request)
	{
		await _authService.Login(request);

		return SendResponse(new { message = "Logged in successfuly." });
	}
}
