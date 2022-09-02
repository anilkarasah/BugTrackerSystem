global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Models;
global using BugTrackerAPI.Data;
global using BugTrackerAPI.Services;

using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
{
	builder.Services.AddControllers().AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	});

	// Connect to SQL server
	//builder.Services.AddDatabaseContext(builder.Configuration);
	builder.Services.AddDbContext<DataContext>(options =>
	{
		//var postgreConnectionString = builder.Configuration.GetConnectionString("PostgreSQL");
		var postgreConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

		var seperators = new string[]
		{
			"postgres://", ":", "@", "/"
		};
		var connectionParameters = postgreConnectionString.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

		var connectionString = $"Server={connectionParameters[2]};Database={connectionParameters[4]};User Id={connectionParameters[0]};Password={connectionParameters[1]};Sslmode=Require;Trust Server Certificate=true";
		
		options.UseNpgsql(connectionString);
		//options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
	});

	// Integrate services
	builder.Services.AddServices(builder.Configuration);
}

var app = builder.Build();
{
	app.UseCors("EnableCORS");
	app.UseExceptionHandler("/error");
	app.UseHttpsRedirection();
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();
	app.Run();
}
