global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Models;
global using BugTrackerAPI.Data;
global using BugTrackerAPI.Services;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
{
	builder.Services.AddControllers().AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	});

	// Connect to SQL server
	builder.Services.AddDbContext<DataContext>(options =>
	{
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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
