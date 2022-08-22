global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Models;
global using BugTrackerAPI.Data;
global using BugTrackerAPI.Services;

using BugTrackerAPI.Common.Authentication.Hash;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using BugTrackerAPI.Common.Authentication.Cookie;

var builder = WebApplication.CreateBuilder(args);
{
	builder.Services.AddControllers();

	// Connect to SQL server
	builder.Services.AddDbContext<DataContext>(options =>
	{
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
	});

	// Integrate services
	builder.Services.AddServices();
	builder.Services.AddSingleton<IHashUtils, HashUtils>();
	builder.Services.AddScoped<ICookieUtils, CookieUtils>();

	// Authentication using cookies
	builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
		{
			options.Cookie.Name = "UserLoginCookie";
			options.SlidingExpiration = true;
			options.LoginPath = "/api/login";
			options.AccessDeniedPath = "/api/login";

			options.Events.OnRedirectToLogin = context =>
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				return Task.CompletedTask;
			};

			options.Events.OnRedirectToAccessDenied = context =>
			{
				context.Response.StatusCode = StatusCodes.Status403Forbidden;
				return Task.CompletedTask;
			};

			options.ExpireTimeSpan = TimeSpan.FromDays(10);
			options.Cookie.HttpOnly = true;
			options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
		});

	// CORS policy for authentication from front-end application
	builder.Services.AddCors(options =>
		options.AddPolicy("Dev", build =>
		{
			build
				//.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()
				.SetIsOriginAllowed(origin =>
				{
					if (string.IsNullOrWhiteSpace(origin)) return false;
					if (origin.ToLower().StartsWith(builder.Configuration["ApplicationUrl:Dev"])) return true;
					if (origin.ToLower().StartsWith(builder.Configuration["ApplicationUrl:Prod"])) return true;
					return false;
				});
		}));
}

var app = builder.Build();
{
	app.UseCors("Dev");
	app.UseExceptionHandler("/error");
	app.UseHttpsRedirection();
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();
	app.Run();
}
