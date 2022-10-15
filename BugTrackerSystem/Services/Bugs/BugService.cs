using BugTrackerAPI.Contracts.Bugs;

namespace BugTrackerAPI.Services;

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
		_context.Entry(request).State = EntityState.Added;

		await Save();
	}

	public async Task<List<Bug>> GetBugs()
	{
		var bugsList = await _context.Bugs
			.AsSplitQuery()
			.Include(b => b.User)
			.Include(b => b.Project)
			.ToListAsync();

		if (bugsList is null || !bugsList.Any())
			throw new ApiException(404, "No bugs found");

		return bugsList;
	}

	public async Task<BugReportData[]> GetMinimalBugData()
	{
		var bugReports = await _context.Bugs
			.Select(b => new BugReportData(b.ID, b.Title))
			.ToArrayAsync();

		return bugReports;
	}

	public async Task<Bug> GetBugByID(int bugID)
	{
		var response = await _context.Bugs
			.AsSplitQuery()
			.Where(b => b.ID == bugID)
			.Include(b => b.User)
			.Include(b => b.Project)
			.FirstOrDefaultAsync();

		if (response is null)
			throw new ApiException(404, $"#{bugID} is not found.");

		return response;
	}

	public async Task UpsertBug(Bug bug)
	{
		_context.Entry(bug).State = EntityState.Modified;
		await Save();
	}

	public async Task DeleteBug(int BugID)
	{
		var bug = await _context.Bugs.FindAsync(BugID);

		if (bug is null)
			throw new ApiException(404, $"BT-{BugID} is not found.");

		_context.Bugs.Remove(bug);
		_context.Entry(bug).State = EntityState.Deleted;

		await Save();
	}

	public async Task Save()
	{
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (Exception e)
		{
			throw new ApiException(500, e.Message);
		}
	}
}
