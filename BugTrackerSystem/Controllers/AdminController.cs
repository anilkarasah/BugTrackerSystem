using Microsoft.AspNetCore.Authorization;

namespace BugTrackerAPI.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : ApiController
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IUserService _userService;

	public AdminController(IAuthorizationService authorizationService, IUserService userService)
	{
		_authorizationService = authorizationService;
		_userService = userService;
	}

	[HttpPatch("grantRole/{id:Guid}")]
	public async Task<IActionResult> UpdateRoleOfUser(Guid id, [FromQuery] string role)
	{
		var user = await _userService.GetUserByID(id);
		if (user is null)
			throw new ApiException(404, $"No user found with ID: {id}");

		if (role != "user" && role != "contributor" && role != "leader" && role != "admin")
			throw new ApiException(400, $"'{role}' is not a valid role.");

		user.Role = role;
		await _userService.UpsertUser(user);

		return SendResponse(user);
	}
}
