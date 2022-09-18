global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Models;
global using BugTrackerAPI.Data;
global using BugTrackerAPI.Services;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
{
	// Listen on 0.0.0.0:$PORT
	// For hosting on Railway
	Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder =>
		builder.ConfigureKestrel(options =>
			options.Listen(System.Net.IPAddress.Parse("0.0.0.0"), int.Parse(Environment.GetEnvironmentVariable("PORT")))
		)
	);
	
	builder.Services.AddControllers().AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	});

	// Integrate services
	builder.Services.AddServices(builder.Configuration);
}

var app = builder.Build();
{
	app.UseHttpsRedirection();
	app.UseCors();
	app.UseExceptionHandler("/error");
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();
	app.Run();
}
