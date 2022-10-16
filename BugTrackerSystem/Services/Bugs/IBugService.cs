using BugTrackerAPI.Contracts.Bugs;

namespace BugTrackerAPI.Services;

public interface IBugService : IService
{
	Task CreateBug(Bug request);
	Task<List<BugResponse>> GetBugs();
	Task<BugReportData[]> GetMinimalBugData();
	Task<BugResponse> GetBugByID(int bugID);
	Task<Bug> UpsertBug(int bugID, UpsertBugRequest request);
	Task DeleteBug(int bugID);
}
