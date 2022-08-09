namespace BugTrackerAPI.Contracts.Projects
{
    public record ProjectResponse(
        Guid ID,
        string Name,
        ICollection<ProjectUser> Contributors,
        ICollection<Bug> Bugs);
}
