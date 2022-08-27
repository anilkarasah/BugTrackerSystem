using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Services;

public interface IAuthService : IService
{
	Task Register(User request);
	Task<User> Login(string email, string password);
	Task<User> GetAuthenticatedUser(HttpContext context);
}
