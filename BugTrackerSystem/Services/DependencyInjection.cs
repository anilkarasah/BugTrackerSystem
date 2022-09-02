using BugTrackerAPI.Common.Authentication.Hash;
using BugTrackerAPI.Common.Authentication.Jwt;
using BugTrackerAPI.Common.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
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
		services.AddDatabaseContext();

		services.AddTransient<IBugService, BugService>();
		services.AddTransient<IProjectService, ProjectService>();
		services.AddTransient<IUserService, UserService>();
		services.AddScoped<IMapperUtils, MapperUtils>();

		return services;
	}

	public static IServiceCollection AddDatabaseContext(
		this IServiceCollection services)
	{
		services.AddDbContext<DataContext>(options =>
		{
			var postgreConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
			var seperators = new string[]
			{
				"postgres://", ":", "@", "/"
			};
			var connectionParameters = postgreConnectionString!.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

			var connectionString = $"Server={connectionParameters[2]};Database={connectionParameters[4]};User Id={connectionParameters[0]};Password={connectionParameters[1]};Sslmode=Require;Trust Server Certificate=true";

			options.UseNpgsql(connectionString);
		});

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
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
				};

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var token = string.Empty;

						if (!string.IsNullOrEmpty(context.Request.Cookies["jwt"]))
						{
							token = context.Request.Cookies["jwt"];
						}
						else if (!string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
						{
							token = context.Request.Headers["Authorization"].ToString().Split(" ").Last();
						}

						context.Token = token;
						return Task.CompletedTask;
					}
				};
			});

		// CORS policy for authentication from front-end application
		services.AddCors(options =>
			options.AddPolicy("EnableCORS", build =>
			{
				build
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()
				.SetIsOriginAllowed(origin =>
				{
					if (string.IsNullOrWhiteSpace(origin)) return false;
					if (origin.ToLower().StartsWith(configuration["ApplicationUrl:Dev"])) return true;
					if (origin.ToLower().StartsWith(configuration["ApplicationUrl:Prod"])) return true;
					return false;
				});
			}));

		return services;
	}
}
