﻿using BugTrackerAPI.Contracts.Bugs;
using BugTrackerAPI.Contracts.Projects;

namespace BugTrackerAPI.Contracts.Users;

public record UserResponse(
	Guid ID,
	string Name,
	string Email,
	string Role,
	int NumberOfContributedProjects,
	ProjectData[] ContributedProjects,
	int NumberOfBugReports,
	BugReportData[] BugReports);
