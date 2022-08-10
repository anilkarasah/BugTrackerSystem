using BugTrackerAPI.Contracts.Bugs;

namespace BugTrackerAPI.Services
{
    public interface IBugService
    {
        Task<string> CreateBug(Bug request);
        IEnumerable<Bug> GetBugs();
        Task<Bug> GetBugByID(Guid bugID);
        Task UpsertBug(Bug request);
        Task DeleteBug(Guid bugID);
        Task Save();
		public BugResponse MapBugResponse(Bug b);
    }
}
