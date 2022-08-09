namespace BugTrackerAPI.Contracts.Projects
{
    public record UpsertProjectRequest(
        Guid ID,
        string Name,
        ICollection<ProjectUser> Contributors,
        ICollection<Bug> Bugs);
}
