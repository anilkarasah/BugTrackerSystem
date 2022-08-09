namespace BugTrackerAPI.Contracts.Bugs
{
    public record CreateBugRequest(
        string Title,
        string Description,
        Guid ProjectID,
        int StatusID,
        string? LogFile);
}
