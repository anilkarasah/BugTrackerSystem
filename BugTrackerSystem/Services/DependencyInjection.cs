using BugTrackerAPI.Common.Authentication.Hash;
using BugTrackerAPI.Common.Authentication.Jwt;
using BugTrackerAPI.Common.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BugTrackerAPI.Services;

public static class DependencyInjection
{
	public static IServiceCollection AddServices(
		this IServiceCollection services,
		ConfigurationManager configuration)
	{
		services.AddAuth(configuration);

		// Connect to SQL server
		services.AddDbContext<DataContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

		services.AddTransient<IBugService, BugService>();
		services.AddTransient<IProjectService, ProjectService>();
		services.AddTransient<IUserService, UserService>();
		services.AddScoped<IMapperUtils, MapperUtils>();

		return services;
	}

	public static IServiceCollection AddAuth(
		this IServiceCollection services,
		ConfigurationManager configuration)
	{
		services.AddTransient<IAuthService, AuthService>();
		services.AddSingleton<IHashUtils, HashUtils>();
		services.AddSingleton<IJwtUtils, JwtUtils>();

		// Authentication using JWT
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,

					ValidIssuer = configuration["Jwt:Issuer"],
					ValidAudience = configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!))
				};

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var token = string.Empty;

						if (!string.IsNullOrWhiteSpace(context.Request.Cookies["jwt"]))
							token = context.HttpContext.Request.Cookies["jwt"];
						else if (!string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
							token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();

						context.Token = token;
						return Task.CompletedTask;
					}
				};
			});

		// CORS policy for authentication from front-end application
		services.AddCors(options =>
			options.AddDefaultPolicy(policy =>
			{
				policy
				.WithOrigins(configuration["ApplicationUrl:Dev"], configuration["ApplicationUrl:Prod"])
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials();
			}));

		return services;
	}
}
