namespace BugTrackerAPI.Services;

public class UserService : IUserService
{
	private readonly DataContext _context;
	public UserService(DataContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<User>> GetAllUsers()
	{
		var usersList = await _context.Users.ToListAsync();

		if (usersList is null || !usersList.Any())
			throw new ApiException(404, "No users found.");

		return usersList;
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
		await _context.SaveChangesAsync();
	}
}
