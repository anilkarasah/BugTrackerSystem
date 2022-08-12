namespace BugTrackerAPI.Services;

public interface IUserService : IService
{
	public Task<IEnumerable<User>> GetAllUsers();
	public Task<User> GetUserByID(Guid userID);
	public Task UpsertUser(User user);
	public Task DeleteUser(Guid userID);
}
