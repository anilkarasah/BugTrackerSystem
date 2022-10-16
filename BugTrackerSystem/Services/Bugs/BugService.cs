using BugTrackerAPI.Common.ValidationAttributes;
using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

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

	public async Task<List<BugResponse>> GetBugs()
	{
		var bugsList = await _context.Bugs
			.AsSplitQuery()
			.Include(b => b.User)
			.Include(b => b.Project)
			.Select(bug => new BugResponse(
				bug.ID,
				bug.Title,
				bug.Description,
				bug.Status,
				new ContributorData(bug.UserID, bug.User.Name),
				new ProjectData(bug.ProjectID, bug.Project.Name),
				bug.CreatedAt,
				bug.LastUpdatedAt
			))
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

	public async Task<BugResponse> GetBugByID(int bugID)
	{
		var response = await _context.Bugs
			.AsSplitQuery()
			.Include(b => b.User)
			.Include(b => b.Project)
			.Select(bug => new BugResponse(
				bug.ID,
				bug.Title,
				bug.Description,
				bug.Status,
				new ContributorData(bug.UserID, bug.User.Name),
				new ProjectData(bug.ProjectID, bug.Project.Name),
				bug.CreatedAt,
				bug.LastUpdatedAt
			))
			.FirstOrDefaultAsync(bug => bug.ID == bugID);

		if (response is null)
			throw new ApiException(404, $"#{bugID} is not found.");

		return response;
	}

	public async Task<Bug> UpsertBug(int bugID, UpsertBugRequest request)
	{
		var bug = await _context.Bugs
			.SingleOrDefaultAsync(b => b.ID == bugID);
		if (bug is null)
			throw new ApiException(404, $"#{bugID} is not found.");
		
		// update contents of the bug report
		bug.Title = !string.IsNullOrWhiteSpace(request.Title)
			? request.Title : bug.Title;
		bug.Description = !string.IsNullOrWhiteSpace(request.Description)
			? request.Description : bug.Description;
		bug.Status = !string.IsNullOrWhiteSpace(request.Status)
			? request.Status : bug.Status;
		bug.LastUpdatedAt = DateTime.UtcNow;
		
		_context.Entry(bug).State = EntityState.Modified;
		await Save();
		
		return bug;
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
