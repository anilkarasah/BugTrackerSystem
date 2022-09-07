using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Services;

public class UserService : IUserService
{
	private readonly DataContext _context;
	public UserService(DataContext context)
	{
		_context = context;
	}

	public async Task<List<User>> GetAllUsers()
	{
		var usersList = await _context.Users.ToListAsync();

		if (usersList is null || !usersList.Any())
			throw new ApiException(404, "No users found.");

		return usersList;
	}

	public async Task<ContributorData[]> GetMinimalUserData()
	{
		var usersData = await _context.Users.Select(u => new ContributorData(u.ID, u.Name)).ToArrayAsync();

		return usersData;
	}

	public async Task<User> GetUserByID(Guid userID)
	{
		var user = await _context.Users.FindAsync(userID);

		if (user is null)
			throw new ApiException(404, $"No user found with ID: {userID}");

		return user;
	}

	public async Task UpsertUser(User user)
	{
		_context.Entry(user).State = EntityState.Modified;
		await Save();
	}

	public async Task DeleteUser(Guid userID)
	{
		var user = await _context.Users.FindAsync(userID);

		if (user is null)
			throw new ApiException(404, $"No user found with ID: {userID}");

		_context.Users.Remove(user);
		_context.Entry(user).State = EntityState.Deleted;

		await Save();
	}

	public async Task Save()
	{
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (Exception e)
		{
			throw new ApiException(500, e.Message);
		}
	}
}
