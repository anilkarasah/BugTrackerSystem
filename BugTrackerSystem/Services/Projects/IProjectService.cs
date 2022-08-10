namespace BugTrackerAPI.Services.Projects
{
	public interface IProjectService
	{
		Task CreateProject(Project project);
		IEnumerable<Project> GetAllProjects();
		Task<Project> GetProjectByID(Guid projectID);
		Task UpsertProject(Project project);
		Task DeleteProject(Guid projectID);
		Task Save();
	}
}
