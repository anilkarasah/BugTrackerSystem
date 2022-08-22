using BugTrackerAPI.Common.Authentication.Hash;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Services;

public class AuthService : IAuthService
{
	private readonly DataContext _context;
	private readonly IHashUtils _hashUtils;
	public AuthService(DataContext context, IHashUtils hashUtils)
	{
		_context = context;
		_hashUtils = hashUtils;
	}

	public async Task Register(User user)
	{
		user.Password = _hashUtils.HashPassword(user.Password);

		await _context.Users.AddAsync(user);
		await Save();
	}

	public async Task<User> Login(string email, string password)
	{
		var candidateUserAccount = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

		if (candidateUserAccount is null || 
			!_hashUtils.VerifyCurrentPassword(password, candidateUserAccount.Password))
			throw new ApiException(400, "Invalid email address or password.");

		return candidateUserAccount;
	}

	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}

	public AuthenticationResponse MapAuthenticationResponse(User user)
	{
		return new AuthenticationResponse(
			user.ID,
			user.Name,
			user.Email,
			user.Role);
	}
}
