using BCryptNet = BCrypt.Net.BCrypt;

namespace BugTrackerAPI.Services;

public class AuthService : IAuthService
{
	private readonly DataContext _context;
	public AuthService(DataContext context)
	{
		_context = context;
	}

	public async Task Register(User user)
	{
		// Hash password by 12 iterations
		user.Password = BCryptNet.HashPassword(inputKey: user.Password, workFactor: 12);

		await _context.Users.AddAsync(user);
		await Save();
	}

	public async Task<User> Login(string email, string password)
	{
		var candidateUserAccount = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

		if (candidateUserAccount is null ||
			!BCryptNet.Verify(password, candidateUserAccount.Password))
			throw new ApiException(400, "Invalid email address or password.");

		return candidateUserAccount;
	}

	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}
}
