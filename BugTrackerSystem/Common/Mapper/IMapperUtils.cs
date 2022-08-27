using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Common.Mapper;

public interface IMapperUtils
{
	AuthenticationResponse MapAuthenticationResponse(User user);
	Task<BugResponse> MapBugResponse(Bug b);
	Task<ProjectResponse> MapProjectResponse(Project project);
	Task<UserResponse> MapUserResponse(User user);
}
