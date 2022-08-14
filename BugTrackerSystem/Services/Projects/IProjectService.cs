namespace BugTrackerAPI.Services;

public interface IProjectService : IService
{
	Task CreateProject(Project project);
	Task<List<Project>> GetAllProjects();
	Task<Project> GetProjectByID(Guid projectID);
	Task UpsertProject(Project project);
	Task DeleteProject(Guid projectID);
	Task<ProjectUser> AddContributor(Guid projectID, Guid contributorID);
}
