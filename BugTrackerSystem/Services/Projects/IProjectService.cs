using BugTrackerAPI.Contracts.Projects;

namespace BugTrackerAPI.Services;

public interface IProjectService : IService
{
	Task CreateProject(Project project);
	Task<List<ProjectResponse>> GetAllProjects();
	Task<object[]> GetMinimalProjectData();
	Task<ProjectResponse> GetProjectByID(Guid projectID);
	Task<Project> UpsertProject(Guid projectID, UpsertProjectRequest request);
	Task DeleteProject(Guid projectID);
	Task<ProjectUser> AddContributor(Guid projectID, Guid contributorID);
	Task RemoveContributor(Guid projectID, Guid contributorID);
	Task<List<User>> GetContributorsList(Guid projectID);
	Task<List<Bug>> GetBugReportsList(Guid projectID);
}
