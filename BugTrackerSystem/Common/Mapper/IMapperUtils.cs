using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;
using BugTrackerAPI.Contracts.Users;

namespace BugTrackerAPI.Common.Mapper;

public interface IMapperUtils
{
	AuthenticationResponse MapAuthenticationResponse(User user);
	BugResponse? MapBugResponse(int bugID);
	ProjectResponse? MapProjectResponse(Guid projectID);
	Task<UserResponse> MapUserResponse(User user);
}
