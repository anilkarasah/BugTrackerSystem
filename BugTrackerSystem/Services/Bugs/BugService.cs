using BugTrackerAPI.Contracts.Bugs;

namespace BugTrackerAPI.Services
{
	public class BugService : IBugService
	{
		private readonly DataContext _context;
		public BugService(DataContext context)
		{
			_context = context;
		}

		public async Task<string> CreateBug(Bug request)
		{
			try
			{
				await _context.AddAsync(request);
				_context.Entry(request).State = EntityState.Added;

				await Save();
				return "OK";
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public IEnumerable<Bug> GetBugs()
		{
			var bugsList = _context.Bugs.ToList();

			if (bugsList is null || bugsList.Count == 0)
				return null;

			return bugsList;
		}

		public async Task<Bug> GetBugByID(Guid BugID)
		{
			var bugResponse = await _context.Bugs.FindAsync(BugID);

			if (bugResponse is null)
				return null;

			bugResponse.Project = await _context.Projects.FindAsync(bugResponse.ProjectID);

			return bugResponse;
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
			_context.Entry(bug).State = EntityState.Deleted;

			await Save();
		}

		public async Task Save()
		{
			await _context.SaveChangesAsync();
		}

		public BugResponse MapBugResponse(Bug b)
		{
			var relatedProject = _context.Projects.Find(b.ProjectID);

			return new BugResponse(
				b.ID,
				b.Title,
				b.Description,
				b.CreatedAt,
				relatedProject,
				b.LogFile);
		}
	}
}
