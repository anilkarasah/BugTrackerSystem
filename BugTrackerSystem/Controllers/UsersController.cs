using BCryptNet = BCrypt.Net.BCrypt;

namespace BugTrackerAPI.Controllers;

public class UsersController : ApiController
{
	private readonly IUserService _userService;
	public UsersController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllUsers()
	{
		var usersList = await _userService.GetAllUsers();
		return SendResponse(usersList);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetUserByID(Guid id)
	{
		var user = await _userService.GetUserByID(id);
		return SendResponse(user);
	}

	[Route("~/api/me")]
	[HttpGet]
	public IActionResult GetProfile()
	{
		return SendResponse(new User
		{
			Email = "anilkarasah@hotmail.com",
			Name = "Anıl Karaşah",
			Role = "user"
		});
	}

	[Route("~/api/me")]
	[HttpPatch]
	public async Task<IActionResult> UpdateMe(UpsertUserRequest request)
	{
		Guid constID = Guid.Parse("099d490b-6115-49e1-93e8-3c337ec5f1f2");
		var loggedInUser = await _userService.GetUserByID(constID);

		loggedInUser.Name = request.Name ?? loggedInUser.Name;
		loggedInUser.Email = request.Email ?? loggedInUser.Email;

		if (request.CurrentPassword is not null && request.NewPassword is not null)
		{
			if (BCryptNet.Verify(request.CurrentPassword, loggedInUser.Password))
				loggedInUser.Password = BCryptNet.HashPassword(request.NewPassword, workFactor: 12);
			else
				throw new ApiException(400, "Current password is wrong.");
		}

		await _userService.UpsertUser(loggedInUser);

		return SendResponse(loggedInUser);
	}
}
