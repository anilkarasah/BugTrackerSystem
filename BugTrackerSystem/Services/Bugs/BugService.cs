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
		var bugsList = await _context.Bugs.ToListAsync();

		if (bugsList is null || bugsList.Count == 0)
			throw new ApiException(404, "No bugs found");

		return bugsList;
	}

	public async Task<Bug> GetBugByID(int BugID)
	{
		var bugResponse = await _context.Bugs
								.Include(b => b.Project)
								.FirstOrDefaultAsync(b => b.ID == BugID);

		if (bugResponse is null)
			throw new ApiException(404, $"BT-{BugID} is not found.");

		//bugResponse.Project = await _context.Projects.Include(p => p.Contibutors).FirstOrDefaultAsync(p => p.ID == bugResponse.ProjectID);

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
		var relatedProject = await _context.Projects.FindAsync(b.ProjectID)!;
		var reporter = await _context.Users.FindAsync(b.UserID)!;

		return new BugResponse(
			b.ID,
			b.Title,
			b.Description,
			b.Status,
			b.UserID,
			reporter?.Name,
			b.ProjectID,
			relatedProject?.Name,
			b.CreatedAt,
			b.LastUpdatedAt);
	}
}
