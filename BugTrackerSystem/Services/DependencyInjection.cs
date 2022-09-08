using BugTrackerAPI.Common.Authentication.Hash;
using BugTrackerAPI.Common.Authentication.Jwt;
using BugTrackerAPI.Common.Mapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
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
		
		// services.AddAuthentication(options => {
		// 	options.DefaultScheme = "JWT_OR_COOKIE";
		// 	options.DefaultChallengeScheme = "JWT_OR_COOKIE";
		// }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
		// 	options.LoginPath = "/login";
		// 	options.ExpireTimeSpan = TimeSpan.FromDays(1);
		// }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
		// 	options.TokenValidationParameters = new TokenValidationParameters
		// 	{
		// 		ValidateIssuer = true,
		// 		ValidateAudience = true,
		// 		ValidateIssuerSigningKey = true,
		// 		ValidateLifetime = true,

		// 		ValidIssuer = configuration["Jwt:Issuer"],
		// 		ValidAudience = configuration["Jwt:Audience"],
		// 		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!))
		// 	};
		// }).AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options => {
		// 	options.ForwardDefaultSelector = context => {
		// 		// does the JWT token provided inside Authorization header?
		// 		string authorization = context.Request.Headers[HeaderNames.Authorization];
		// 		if (!string.IsNullOrWhiteSpace(authorization) && authorization.StartsWith("Bearer "))
		// 			return JwtBearerDefaults.AuthenticationScheme;
				
		// 		// then it must be provided through cookies
		// 		return CookieAuthenticationDefaults.AuthenticationScheme;
		// 	};
		// });

		// CORS policy for authentication from front-end application
		services.AddCors(options =>
			options.AddPolicy("EnableCORS", build =>
			{
				build
				.WithOrigins(configuration["ApplicationUrl:Dev"], configuration["ApplicationUrl:Prod"])
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials();
				// .SetIsOriginAllowed(origin =>
				// {
				// 	Console.WriteLine($"------> {origin}");
				// 	if (string.IsNullOrWhiteSpace(origin)) return false;
				// 	if (origin.ToLower().StartsWith(configuration["ApplicationUrl:Dev"])) return true;
				// 	if (origin.ToLower().StartsWith(configuration["ApplicationUrl:Prod"])) return true;
				// 	return false;
				// });
			}));

		return services;
	}
}
