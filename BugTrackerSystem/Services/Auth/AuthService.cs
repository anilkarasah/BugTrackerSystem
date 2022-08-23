using BugTrackerAPI.Common.Authentication.Hash;
using BugTrackerAPI.Common.Authentication.Jwt;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Services;

public class AuthService : IAuthService
{
	private readonly DataContext _context;
	private readonly IHashUtils _hashUtils;
	private readonly IJwtUtils _jwtUtils;
	public AuthService(DataContext context, IHashUtils hashUtils, IJwtUtils jwtUtils)
	{
		_context = context;
		_hashUtils = hashUtils;
		_jwtUtils = jwtUtils;
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

	public async Task<User> GetAuthenticatedUser(HttpContext context)
	{
		var token = context.Request.Cookies["jwt"];
		var userID = _jwtUtils.ValidateToken(token);

		var user = await _context.Users.FindAsync(userID);
		if (user is null)
			throw new ApiException(401, "You are not authenticated. Please log in again.");

		return user;
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
