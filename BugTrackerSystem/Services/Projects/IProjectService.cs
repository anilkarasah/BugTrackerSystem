using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;

namespace BugTrackerAPI.Services;

public interface IProjectService : IService
{
	Task CreateProject(Project project);
	Task<List<Project>> GetAllProjects();
	Task<ProjectData[]> GetMinimalProjectData();
	Task<Project> GetProjectByID(Guid projectID);
	Task UpsertProject(Project project);
	Task DeleteProject(Guid projectID);
	Task<ProjectUser> AddContributor(Guid projectID, Guid contributorID);
	Task RemoveContributor(Guid projectID, Guid contributorID);
	Task<List<User>> GetContributorsList(Guid projectID);
	Task<List<Bug>> GetBugReportsList(Guid projectID);
	Task<ProjectResponse> MapProjectResponse(Project project);
}
