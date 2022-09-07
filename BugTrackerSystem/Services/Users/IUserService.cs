﻿using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Services;

public interface IUserService : IService
{
	public Task<List<User>> GetAllUsers();
	Task<ContributorData[]> GetMinimalUserData();
	public Task<User> GetUserByID(Guid userID);
	public Task UpsertUser(User user);
	public Task DeleteUser(Guid userID);
}
