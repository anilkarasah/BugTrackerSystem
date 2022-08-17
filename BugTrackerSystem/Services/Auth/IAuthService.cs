using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Services;

public interface IAuthService : IService
{
	Task Register(User request);
	Task<User> Login(string email, string password);
	AuthenticationResponse MapAuthenticationResponse(User user, string? token);
}
