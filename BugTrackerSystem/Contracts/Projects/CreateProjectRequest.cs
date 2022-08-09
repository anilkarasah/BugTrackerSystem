namespace BugTrackerAPI.Contracts.Projects
{
    public record CreateProjectRequest(
        Guid ID,
        string Name,
        ICollection<ProjectUser> Contributors,
        ICollection<Bug> Bugs);
}
