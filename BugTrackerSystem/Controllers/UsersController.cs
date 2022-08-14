using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using BCryptNet = BCrypt.Net.BCrypt;

namespace BugTrackerAPI.Controllers;

[Authorize]
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
		var loggedInUser = (User)HttpContext.Items["User"]!;

		return SendResponse(new User
		{
			ID = loggedInUser.ID,
			Email = loggedInUser.Email,
			Name = loggedInUser.Name,
			Role = loggedInUser.Role,
			ProjectsList = loggedInUser.ProjectsList
		});
	}

	[Route("~/api/me")]
	[HttpPatch]
	public async Task<IActionResult> UpdateMe(UpsertUserRequest request)
	{
		var loggedInUser = (User)HttpContext.Items["User"];

		if (loggedInUser is null)
			throw new ApiException(401, "You are unauthorized. Please log in.");

		// did user provide name or email
		loggedInUser.Name ??= request.Name;
		loggedInUser.Email ??= request.Email;

		// is user trying to update password
		// it requires both currentPassword and newPassword fields
		if (request.CurrentPassword is not null && request.NewPassword is not null)
		{
			if (BCryptNet.Verify(request.CurrentPassword, loggedInUser.Password))
				loggedInUser.Password = BCryptNet.HashPassword(inputKey: request.NewPassword, workFactor: 12);
			else
				throw new ApiException(400, "Current password is wrong.");
		}

		await _userService.UpsertUser(loggedInUser);

		return SendResponse(loggedInUser);
	}
}
