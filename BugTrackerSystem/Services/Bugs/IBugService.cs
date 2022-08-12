using BugTrackerAPI.Contracts.Bugs;

namespace BugTrackerAPI.Services;

public interface IBugService : IService
{
	Task CreateBug(Bug request);
	Task<List<Bug>> GetBugs();
	Task<Bug> GetBugByID(Guid bugID);
	Task UpsertBug(Bug request);
	Task DeleteBug(Guid bugID);
	public BugResponse MapBugResponse(Bug b);
}
