namespace BugTrackerAPI.Services
{
    public interface IProjectService
    {
        void CreateProject(Project project);
        IEnumerable<Project> GetAllProjects();
        Task<Project> GetProjectByID(Guid projectID);
        void UpdateProject(Project project);
        Task DeleteProject(Guid projectID);
        void Save();
    }
}
