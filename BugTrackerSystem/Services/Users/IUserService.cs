using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Services;

public interface IUserService : IService
{
	public Task<IEnumerable<User>> GetAllUsers();
	Task<ContributorData[]> GetMinimalUserData();
	public Task<User> GetUserByID(Guid userID);
	public Task UpsertUser(User user);
	public Task DeleteUser(Guid userID);
	public Task<UserResponse> MapUserResponse(User user);
}
