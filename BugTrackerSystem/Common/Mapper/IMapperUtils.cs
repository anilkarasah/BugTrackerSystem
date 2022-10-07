using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Common.Mapper;

public interface IMapperUtils
{
	AuthenticationResponse MapAuthenticationResponse(User user);
	void MapBugResponse(int bugID);
	Task<ProjectResponse> MapProjectResponse(Project project);
	Task<UserResponse> MapUserResponse(User user);
}
