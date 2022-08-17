using BugTrackerAPI.Common.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerAPI.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : ApiController
{
	private readonly IUserService _userService;

	public AdminController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpPatch("grantRole/{id:Guid}")]
	public async Task<IActionResult> UpdateRoleOfUser(Guid id, [FromQuery] string role)
	{
		var user = await _userService.GetUserByID(id);

		if (CheckRole.IsRoleValid(role))
			user.Role = role.ToLower();
		else
			throw new ApiException(400, $"'{role}' is not a valid role.");

		await _userService.UpsertUser(user);

		return SendResponse(user);
	}

	[HttpDelete("deleteUser/{userID:Guid}")]
	public async Task<IActionResult> DeleteUser(Guid userID)
	{
		await _userService.DeleteUser(userID);
		return SendResponse(null, 204);
	}
}
