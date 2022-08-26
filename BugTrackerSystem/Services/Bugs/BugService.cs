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
		try
		{
			await _context.AddAsync(request);
			_context.Entry(request).State = EntityState.Added;

			await Save();
		}
		catch (Exception e)
		{
			throw new ApiException(500, e.Message);
		}
	}

	public async Task<List<Bug>> GetBugs()
	{
		var bugsList = await _context.Bugs.ToListAsync();

		if (bugsList is null || bugsList.Count == 0)
			throw new ApiException(404, "No bugs found");

		return bugsList;
	}

	public async Task<BugReportData[]> GetMinimalBugData()
	{
		var bugReports = await _context.Bugs.Select(b => new BugReportData(b.ID, b.Title)).ToArrayAsync();

		return bugReports;
	}

	public async Task<Bug> GetBugByID(int BugID)
	{
		var bugResponse = await _context.Bugs
								.Include(b => b.Project)
								.FirstOrDefaultAsync(b => b.ID == BugID);

		if (bugResponse is null)
			throw new ApiException(404, $"BT-{BugID} is not found.");

		return bugResponse;
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

	public async Task<BugResponse> MapBugResponse(Bug b)
	{
		var project = await _context.Projects
								.Select(p => new { p.ID, p.Name })
								.FirstAsync(p => p.ID == b.ProjectID);

		var reporter = await _context.Users
								.Select(u => new { u.ID, u.Name })
								.FirstAsync(u => u.ID == b.UserID);

		return new BugResponse(
			b.ID,
			b.Title,
			b.Description,
			b.Status,
			b.ProjectID,
			reporter.Name,
			b.UserID,
			project.Name,
			b.CreatedAt.ToString("ddd, MMM d, yyy", new System.Globalization.CultureInfo("en-US")),
			b.LastUpdatedAt.ToString("ddd, MMM d, yyy", new System.Globalization.CultureInfo("en-US")));
	}
}
