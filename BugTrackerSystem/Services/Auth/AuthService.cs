using BugTrackerAPI.Contracts.Users;
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
		user.Password = BCryptNet.HashPassword(input: user.Password, workFactor: 12);

		await _context.Users.AddAsync(user);
		await Save();
	}

	public async Task Login(User user)
	{
		var candidateUserAccount = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

		if (candidateUserAccount is null ||
			!BCryptNet.Verify(user.Password, candidateUserAccount.Password))
			throw new ApiException(400, "Invalid email address or password.");
	}

	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}
}
