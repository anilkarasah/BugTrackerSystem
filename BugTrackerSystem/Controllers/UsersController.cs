﻿using BugTrackerAPI.Common.Authentication.Hash;
using BugTrackerAPI.Common.Mapper;
using BugTrackerAPI.Contracts.Users;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerAPI.Controllers;

[Authorize]
public class UsersController : ApiController
{
	private readonly IUserService _userService;
	private readonly IHashUtils _hashUtils;
	private readonly IAuthService _authService;
	public UsersController(IUserService userService, IHashUtils hashUtils, IAuthService authService)
	{
		_userService = userService;
		_hashUtils = hashUtils;
		_authService = authService;
	}

	[HttpGet]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> GetAllUsers()
	{
		var usersList = await _userService.GetAllUsers();

		var usersListResponse = MapperUtils.MapAllUserResponses(usersList);
		return SendResponse(usersListResponse);
	}

	[HttpGet("mini")]
	public async Task<IActionResult> GetMinimalUserData()
	{
		var minimalUsersData = await _userService.GetMinimalUserData();

		return SendResponse(minimalUsersData);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetUserByID(Guid id)
	{
		var user = await _userService.GetUserByID(id);

		var response = MapperUtils.MapUserResponse(user);
		return SendResponse(response);
	}

	// PROFILE ENDPOINTS
	[Route("~/api/me")]
	[HttpGet]
	public async Task<IActionResult> GetProfile()
	{
		var loggedInUser = await _authService.GetAuthenticatedUser(HttpContext);

		var response = MapperUtils.MapUserResponse(loggedInUser);
		return SendResponse(response);
	}

	[Route("~/api/me")]
	[HttpPatch]
	public async Task<IActionResult> UpdateMe(UpsertUserRequest request)
	{
		var loggedInUser = await _authService.GetAuthenticatedUser(HttpContext);

		if (loggedInUser is null)
			throw new ApiException(401, "You are unauthorized. Please log in.");

		// did user provide name or email
		if (!string.IsNullOrWhiteSpace(request.Name))
			loggedInUser.Name = request.Name.Trim();
		
		if (!string.IsNullOrWhiteSpace(request.Email))
			loggedInUser.Email = request.Email.Trim();

		// is user trying to update password
		// it requires both currentPassword and newPassword fields
		if (!string.IsNullOrWhiteSpace(request.CurrentPassword)
			&& !string.IsNullOrWhiteSpace(request.NewPassword))
		{
			if (_hashUtils.VerifyCurrentPassword(request.CurrentPassword, loggedInUser.Password))
				loggedInUser.Password = _hashUtils.HashPassword(request.NewPassword);
			else
				throw new ApiException(400, "Current password is wrong.");
		}

		await _userService.UpsertUser(loggedInUser);

		var response = MapperUtils.MapUserResponse(loggedInUser);
		return SendResponse(response);
	}

	// ADMIN-ONLY ENDPOINTS
	[Authorize(Roles = "admin")]
	[HttpPatch("{id:Guid}")]
	public async Task<IActionResult> UpdateUserInformation(Guid id, AdminUpsertUserRequest request)
	{
		var user = await _userService.GetUserByID(id);

		user.Name = string.IsNullOrEmpty(request.Name) ? user.Name : request.Name.Trim();
		user.Email = string.IsNullOrEmpty(request.Email) ? user.Email : request.Email.Trim();
		user.Role = string.IsNullOrEmpty(request.Role) ? user.Role : request.Role.ToLower().Trim();

		if (!string.IsNullOrEmpty(request.NewPassword))
			user.Password = _hashUtils.HashPassword(request.NewPassword);

		await _userService.UpsertUser(user);

		var response = MapperUtils.MapUserResponse(user);
		return SendResponse(response);
	}

	[Authorize(Roles = "admin")]
	[HttpDelete("{userID:Guid}")]
	public async Task<IActionResult> DeleteUser(Guid userID)
	{
		await _userService.DeleteUser(userID);
		return SendResponse(null, 204);
	}
}
