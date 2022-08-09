namespace BugTrackerAPI.Contracts.Bugs
{
    public record UpsertBugRequest(
        string Title,
        string Description,
        DateTime CreatedAt,
        Guid ProjectID,
        int StatusID,
        string? LogFile);
}
