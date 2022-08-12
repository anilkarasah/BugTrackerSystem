namespace BugTrackerAPI.Services;

public static class DependencyInjection
{
	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddTransient<IBugService, BugService>();
		services.AddTransient<IProjectService, ProjectService>();
		services.AddTransient<IAuthService, AuthService>();
		services.AddTransient<IUserService, UserService>();

		return services;
	}
}
