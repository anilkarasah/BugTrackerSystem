using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Services;

public interface IAuthService : IService
{
	Task Register(User request);
	Task Login(User request);
}
