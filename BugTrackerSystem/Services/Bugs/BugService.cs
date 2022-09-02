﻿using BugTrackerAPI.Contracts.Bugs;

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
		bool anyBugs = await _context.Bugs.AnyAsync();
		if (!anyBugs)
			throw new ApiException(404, "No bugs found");

		var bugsList = await _context.Bugs.ToListAsync();
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
}
