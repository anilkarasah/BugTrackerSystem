using BugTrackerAPI.ServiceErrors;

namespace BugTrackerAPI.Services
{
    public class BugService : IBugService
    {
        private readonly DataContext _context;
        public BugService(DataContext context)
        {
            _context = context;
        }

        public async Task CreateBug(Bug request)
        {
            await _context.AddAsync(request);
            await Save();
        }

        public ErrorOr<IEnumerable<Bug>> GetBugs()
        {
            var bugsList = _context.Bugs.Include(b => b.Project).ToList();

            if (bugsList.Count == 0)
                return Errors.Bug.NotFound;

            return bugsList;
        }

        public async Task<ErrorOr<Bug>> GetBugByID(Guid BugID)
        {
            ErrorOr<Bug> bugResponse = await _context.Bugs.FindAsync(BugID);

            if (bugResponse.Value is null)
                return Errors.Bug.NotFound;

            return bugResponse.Value;
        }

        public async Task UpsertBug(Bug bug)
        {
            _context.Entry(bug).State = EntityState.Modified;
            await Save();
        }

        public async Task DeleteBug(Guid BugID)
        {
            Bug? bug = await _context.Bugs.FindAsync(BugID);

            if (bug is null)
                return;

            _context.Bugs.Remove(bug);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
