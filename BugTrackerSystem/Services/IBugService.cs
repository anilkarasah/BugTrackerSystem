namespace BugTrackerAPI.Services
{
    public interface IBugService
    {
        Task CreateBug(Bug request);
        ErrorOr<IEnumerable<Bug>> GetBugs();
        Task<ErrorOr<Bug>> GetBugByID(Guid bugID);
        Task UpsertBug(Bug request);
        Task DeleteBug(Guid bugID);
        Task Save();
    }
}
